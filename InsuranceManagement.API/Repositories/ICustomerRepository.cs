using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Repositories;

/// <summary>Defines persistence operations for insurance customers.</summary>
public interface ICustomerRepository
{
    /// <summary>Gets a filtered and ordered page of customers.</summary>
    /// <param name="search">Optional search text matched against customer name, email, or phone.</param>
    /// <param name="pageNumber">One-based page number.</param>
    /// <param name="pageSize">Maximum records to return.</param>
    /// <param name="sortBy">Supported sort field: name or email.</param>
    /// <param name="sortDescending">Whether to sort in descending order.</param>
    /// <param name="cancellationToken">Token used to cancel the database operation.</param>
    Task<(IReadOnlyList<Customer> Items, int TotalCount)> SearchAsync(
        string? search,
        int pageNumber,
        int pageSize,
        string sortBy,
        bool sortDescending,
        CancellationToken cancellationToken);

    /// <summary>Gets a customer by its identifier.</summary>
    /// <param name="id">The customer identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the database operation.</param>
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken);

    /// <summary>Adds a customer and persists it to the database.</summary>
    /// <param name="customer">The customer to add.</param>
    /// <param name="cancellationToken">Token used to cancel the database operation.</param>
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);

    /// <summary>Replaces a customer's editable fields and persists the changes.</summary>
    /// <param name="customer">The customer values to save, including its identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the database operation.</param>
    /// <returns>The updated customer, or <see langword="null"/> if it does not exist.</returns>
    Task<Customer?> UpdateAsync(Customer customer, CancellationToken cancellationToken);

    /// <summary>Deletes a customer by its identifier.</summary>
    /// <param name="id">The customer identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the database operation.</param>
    /// <returns><see langword="true"/> if the customer was deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
