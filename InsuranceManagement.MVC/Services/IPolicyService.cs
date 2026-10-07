using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Services;

public interface IPolicyService
{
    Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken);
    Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> CreateAsync(Policy policy, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, Policy changes, CancellationToken cancellationToken);
    Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
