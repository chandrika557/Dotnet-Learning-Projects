using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Repositories;

/// <summary>Defines persistence operations for insurance claims.</summary>
public interface IClaimRepository
{
    /// <summary>Gets all claims in submission-date order.</summary>
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Gets a claim by its identifier.</summary>
    Task<Claim?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Adds a claim and persists it.</summary>
    Task<Claim> AddAsync(Claim claim, CancellationToken cancellationToken);

    /// <summary>Changes a claim's status and persists it.</summary>
    Task<Claim?> UpdateStatusAsync(int id, ClaimStatus status, CancellationToken cancellationToken);
}
