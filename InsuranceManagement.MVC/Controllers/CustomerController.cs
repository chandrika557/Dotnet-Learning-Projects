using InsuranceManagement.MVC.Models; //model represents database tables
using InsuranceManagement.MVC.Services;
using InsuranceManagement.MVC.ViewModels; //Database Entity → ViewModel → View
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagement.MVC.Controllers;

public class CustomerController : Controller // controller class for customer management
{
    private readonly ICustomerService _customerService;  // private field to hold the customer service instance

    public CustomerController(ICustomerService customerService) // constructor injection for customer service
    {
        _customerService = customerService; 
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) // method to retrieve all customers and display them in the view
    {
        var customers = await _customerService.GetAllAsync(cancellationToken); // Retrieve all customers from the service

        var customerViewModels = customers
            .Select(customer => new CustomerListItemViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            })
            .ToList();

        return View(new CustomerListViewModel { Customers = customerViewModels }); // Pass the list of customer view models to the view
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return View(ToDetailsViewModel(customer)); // Convert the Customer model to a CustomerDetailsViewModel and pass it to the view
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CustomerFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // to prevent CSRF attacks
    public async Task<IActionResult> Create( // method to create a new customer
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

        await _customerService.CreateAsync(customer, cancellationToken); // Call the CreateAsync method of the customer service to create a new customer

        TempData["SuccessMessage"] = "Customer created successfully."; // Store a success message in TempData to display it on the next page
        return RedirectToAction(nameof(Index)); // Redirect to the Index action to display the list of customers
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken) // method to retrieve a customer by ID and display the edit form
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken); // Retrieve the customer by ID from the service

        if (customer is null) //check if the customer is null, which means that the customer with the specified ID does not exist
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
        var customer = await _customerService.GetByIdAsync(id, cancellationToken); //

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
        return RedirectToAction(nameof(Index)); //redirect to the Index action to display the list of customers after successful deletion
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
