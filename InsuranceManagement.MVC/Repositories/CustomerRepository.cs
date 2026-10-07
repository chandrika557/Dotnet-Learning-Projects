using InsuranceManagement.MVC.Data;
using InsuranceManagement.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.MVC.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Customers // Retrieve all customers from the database
            .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
            .OrderBy(customer => customer.Name)
            .ThenBy(customer => customer.Id) // Order the customers by Name and then by Id to ensure a consistent order
            .ToListAsync(cancellationToken); // Execute the query asynchronously and return the list of customers
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken)
    {
        return _context.Policies // Check if the customer has any associated policies by querying the Policies DbSet
            .AnyAsync(policy => policy.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken); // Add the new customer to the Customers DbSet and save changes to the database
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Customers.Update(customer); // Update the existing customer in the Customers DbSet and save changes to the database
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
