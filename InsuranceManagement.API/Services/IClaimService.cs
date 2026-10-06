using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Services;

/// <summary>Defines insurance claim use cases for the API.</summary>
public interface IClaimService
{
    Task<IReadOnlyList<ClaimResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<ClaimResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ClaimResponse> CreateAsync(CreateClaimRequest request, CancellationToken cancellationToken);
    Task<ClaimResponse?> UpdateStatusAsync(
        int id,
        UpdateClaimStatusRequest request,
        CancellationToken cancellationToken);
}
