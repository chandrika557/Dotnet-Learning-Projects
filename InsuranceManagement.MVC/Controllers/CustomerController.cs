using InsuranceManagement.MVC.Models; //model represents database tables
using InsuranceManagement.MVC.Services;
using InsuranceManagement.MVC.ViewModels; //Database Entity → ViewModel → View
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagement.MVC.Controllers;

public class CustomerController : Controller // controller class for customer management
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);

        var customerViewModels = customers
            .Select(customer => new CustomerListItemViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            })
            .ToList();

        return View(new CustomerListViewModel { Customers = customerViewModels });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return View(ToDetailsViewModel(customer));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CustomerFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CustomerFormViewModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        var customer = new Customer
        {
            Name = input.Name,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address
        };

        await _customerService.CreateAsync(customer, cancellationToken);

        TempData["SuccessMessage"] = "Customer created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return View(new CustomerFormViewModel
        {
            Name = customer.Name ?? string.Empty,
            Email = customer.Email ?? string.Empty,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CustomerFormViewModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        var updated = await _customerService.UpdateAsync(id, new Customer
        {
            Name = input.Name,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address
        }, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Customer updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        var viewModel = ToDetailsViewModel(customer);
        viewModel = new CustomerDetailsViewModel
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Email = viewModel.Email,
            PhoneNumber = viewModel.PhoneNumber,
            Address = viewModel.Address,
            HasPolicies = await _customerService.HasPoliciesAsync(id, cancellationToken)
        };
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        if (await _customerService.GetByIdAsync(id, cancellationToken) is null)
        {
            return NotFound();
        }

        if (await _customerService.HasPoliciesAsync(id, cancellationToken))
        {
            TempData["ErrorMessage"] = "This customer has policies and cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        var deleted = await _customerService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Customer deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private static CustomerDetailsViewModel ToDetailsViewModel(Customer customer)
    {
        return new CustomerDetailsViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        };
    }
}
