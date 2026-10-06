using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Repositories;

public interface ICustomerRepository // Interface for customer repository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken); //method to retrieve all customers from the repository
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    Task DeleteAsync(Customer customer, CancellationToken cancellationToken);
}
