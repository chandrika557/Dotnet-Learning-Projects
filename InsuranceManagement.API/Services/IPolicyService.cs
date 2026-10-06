using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Services;

/// <summary>Defines policy use cases for the API.</summary>
public interface IPolicyService
{
    Task<IReadOnlyList<PolicyResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<PolicyResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PolicyResponse> CreateAsync(CreatePolicyRequest request, CancellationToken cancellationToken);
    Task<PolicyResponse?> UpdateAsync(int id, UpdatePolicyRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
