using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Services;

/// <summary>Defines policy use cases for the API.</summary>
public interface IPolicyService
{
    /// <summary>Gets all policy response DTOs.</summary>
    Task<IReadOnlyList<PolicyResponse>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Gets a policy response or null when the ID does not exist.</summary>
    Task<PolicyResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Creates a policy for an existing customer.</summary>
    Task<PolicyResponse> CreateAsync(CreatePolicyRequest request, CancellationToken cancellationToken);

    /// <summary>Replaces policy details or returns null when the ID does not exist.</summary>
    Task<PolicyResponse?> UpdateAsync(int id, UpdatePolicyRequest request, CancellationToken cancellationToken);

    /// <summary>Deletes a policy and returns whether it existed.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
