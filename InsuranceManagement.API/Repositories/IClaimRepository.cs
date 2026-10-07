using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Repositories;

/// <summary>Defines persistence operations for insurance claims.</summary>
public interface IClaimRepository
{
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken);
    Task<Claim?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Claim> AddAsync(Claim claim, CancellationToken cancellationToken);
    Task<Claim?> UpdateStatusAsync(int id, ClaimStatus status, CancellationToken cancellationToken);
}
