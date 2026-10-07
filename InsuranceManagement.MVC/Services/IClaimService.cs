using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Services;

public interface IClaimService
{
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken);
    Task<ClaimCreationResult> CreateAsync(Claim claim, CancellationToken cancellationToken);
    Task<bool> SetStatusAsync(int id, ClaimStatus status, CancellationToken cancellationToken);
}
