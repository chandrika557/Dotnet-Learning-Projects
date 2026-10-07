using InsuranceManagement.MVC.Data;
using InsuranceManagement.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.MVC.Repositories;

public sealed class ClaimRepository : IClaimRepository
{
    private readonly ApplicationDbContext _context;

    public ClaimRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Claims
            .AsNoTracking()
            .Include(claim => claim.Policy)
            .ThenInclude(policy => policy.Customer)
            .OrderByDescending(claim => claim.ClaimDate)
            .ThenByDescending(claim => claim.ClaimId)
            .ToListAsync(cancellationToken);
    }

    public Task<Claim?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Claims
            .FirstOrDefaultAsync(claim => claim.ClaimId == id, cancellationToken);
    }

    public async Task AddAsync(Claim claim, CancellationToken cancellationToken)
    {
        await _context.Claims.AddAsync(claim, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Claim claim, CancellationToken cancellationToken)
    {
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
