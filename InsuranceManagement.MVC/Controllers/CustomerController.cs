using InsuranceManagement.MVC.Data;
using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.MVC.Controllers;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Name)
            .ThenBy(customer => customer.Id)
            .Select(customer => new CustomerListItemViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            })
            .ToListAsync(cancellationToken);

        return View(new CustomerListViewModel { Customers = customers });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == id)
            .Select(customer => new CustomerDetailsViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            })
            .FirstOrDefaultAsync(cancellationToken);

        return customer is null ? NotFound() : View(customer);
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

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["SuccessMessage"] = "Customer created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);

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

        var customer = await _context.Customers.FindAsync([id], cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        customer.Name = input.Name;
        customer.Email = input.Email;
        customer.PhoneNumber = input.PhoneNumber;
        customer.Address = input.Address;

        await _context.SaveChangesAsync(cancellationToken);

        TempData["SuccessMessage"] = "Customer updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == id)
            .Select(customer => new CustomerDetailsViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            })
            .FirstOrDefaultAsync(cancellationToken);

        return customer is null ? NotFound() : View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync([id], cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["SuccessMessage"] = "Customer deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
