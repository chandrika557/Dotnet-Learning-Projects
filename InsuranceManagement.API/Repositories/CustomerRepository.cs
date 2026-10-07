using InsuranceManagement.API.Data;
using InsuranceManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.API.Repositories;

/// <summary>Implements customer persistence operations with Entity Framework Core.</summary>
public sealed class CustomerRepository(ApplicationDbContext dbContext) : ICustomerRepository
{
    /// <inheritdoc />
    public async Task<(IReadOnlyList<Customer> Items, int TotalCount)> SearchAsync(
        string? search,
        int pageNumber,
        int pageSize,
        string sortBy,
        bool sortDescending,
        CancellationToken cancellationToken)
    {
        IQueryable<Customer> query = dbContext.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(customer =>
                customer.Name.Contains(term)
                || (customer.Email != null && customer.Email.Contains(term))
                || (customer.PhoneNumber != null && customer.PhoneNumber.Contains(term)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        query = (sortBy.ToLowerInvariant(), sortDescending) switch
        {
            ("email", false) => query.OrderBy(customer => customer.Email).ThenBy(customer => customer.Id),
            ("email", true) => query.OrderByDescending(customer => customer.Email).ThenBy(customer => customer.Id),
            ("name", true) => query.OrderByDescending(customer => customer.Name).ThenBy(customer => customer.Id),
            _ => query.OrderBy(customer => customer.Name).ThenBy(customer => customer.Id)
        };

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    /// <inheritdoc />
    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken) =>
        dbContext.Policies.AnyAsync(policy => policy.CustomerId == customerId, cancellationToken);

    /// <inheritdoc />
    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return customer;
    }

    /// <inheritdoc />
    public async Task<Customer?> UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        var existingCustomer = await dbContext.Customers
            .FirstOrDefaultAsync(existing => existing.Id == customer.Id, cancellationToken);

        if (existingCustomer is null)
        {
            return null;
        }

        existingCustomer.Name = customer.Name;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;
        existingCustomer.Address = customer.Address;

        await dbContext.SaveChangesAsync(cancellationToken);

        return existingCustomer;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(existing => existing.Id == id, cancellationToken);

        if (customer is null)
        {
            return false;
        }

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
