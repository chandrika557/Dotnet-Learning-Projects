using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Repositories;

/// <summary>Defines persistence operations for insurance policies.</summary>
public interface IPolicyRepository
{
    /// <summary>Gets all policies in name order.</summary>
    Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Gets a policy by its identifier.</summary>
    Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Checks whether a policy has associated claims.</summary>
    Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken);

    /// <summary>Adds a policy and persists it.</summary>
    Task<Policy> AddAsync(Policy policy, CancellationToken cancellationToken);

    /// <summary>Updates a policy and returns null when it does not exist.</summary>
    Task<Policy?> UpdateAsync(Policy policy, CancellationToken cancellationToken);

    /// <summary>Deletes a policy and returns whether it existed.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
