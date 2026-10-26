using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Services;

/// <summary>Defines insurance claim use cases for the API.</summary>
public interface IClaimService
{
    /// <summary>Gets all claim response DTOs.</summary>
    Task<IReadOnlyList<ClaimResponse>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Gets a claim response or null when the ID does not exist.</summary>
    Task<ClaimResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Submits a pending claim against an existing policy.</summary>
    Task<ClaimResponse> CreateAsync(CreateClaimRequest request, CancellationToken cancellationToken);

    /// <summary>Reviews a pending claim, returning null when the ID does not exist.</summary>
    Task<ClaimResponse?> UpdateStatusAsync(
        int id,
        UpdateClaimStatusRequest request,
        CancellationToken cancellationToken);
}
