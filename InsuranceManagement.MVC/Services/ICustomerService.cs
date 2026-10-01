using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken);
    Task CreateAsync(Customer customer, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, Customer changes, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
