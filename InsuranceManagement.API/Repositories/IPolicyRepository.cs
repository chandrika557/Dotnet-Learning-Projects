using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Repositories;

/// <summary>Defines persistence operations for insurance policies.</summary>
public interface IPolicyRepository
{
    Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken);
    Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken);
    Task<Policy> AddAsync(Policy policy, CancellationToken cancellationToken);
    Task<Policy?> UpdateAsync(Policy policy, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
