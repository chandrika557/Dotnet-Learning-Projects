using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Repositories;

public interface IPolicyRepository
{
    Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken);
    Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken);
    Task AddAsync(Policy policy, CancellationToken cancellationToken);
    Task UpdateAsync(Policy policy, CancellationToken cancellationToken);
    Task DeleteAsync(Policy policy, CancellationToken cancellationToken);
}
