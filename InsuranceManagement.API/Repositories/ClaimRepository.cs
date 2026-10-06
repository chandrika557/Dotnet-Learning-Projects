using InsuranceManagement.API.Data;
using InsuranceManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.API.Repositories;

/// <summary>Implements claim persistence operations with Entity Framework Core.</summary>
public sealed class ClaimRepository(ApplicationDbContext dbContext) : IClaimRepository
{
    public async Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Claims.AsNoTracking()
            .OrderByDescending(claim => claim.ClaimDate)
            .ThenBy(claim => claim.ClaimId)
            .ToListAsync(cancellationToken);

    public Task<Claim?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Claims.AsNoTracking()
            .FirstOrDefaultAsync(claim => claim.ClaimId == id, cancellationToken);

    public async Task<Claim> AddAsync(Claim claim, CancellationToken cancellationToken)
    {
        await dbContext.Claims.AddAsync(claim, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return claim;
    }

    public async Task<Claim?> UpdateStatusAsync(
        int id,
        ClaimStatus status,
        CancellationToken cancellationToken)
    {
        var claim = await dbContext.Claims.FirstOrDefaultAsync(
            item => item.ClaimId == id, cancellationToken);
        if (claim is null)
        {
            return null;
        }

        claim.Status = status;
        await dbContext.SaveChangesAsync(cancellationToken);
        return claim;
    }
}
