using InsuranceManagement.MVC.Data;
using InsuranceManagement.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.MVC.Repositories;

public sealed class PolicyRepository : IPolicyRepository
{
    private readonly ApplicationDbContext _context;

    public PolicyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Policies
            .AsNoTracking()
            .Include(policy => policy.Customer)
            .OrderBy(policy => policy.PolicyName)
            .ThenBy(policy => policy.PolicyId)
            .ToListAsync(cancellationToken);
    }

    public Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Policies
            .AsNoTracking()
            .Include(policy => policy.Customer)
            .FirstOrDefaultAsync(policy => policy.PolicyId == id, cancellationToken);
    }

    public Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken)
    {
        return _context.Claims
            .AnyAsync(claim => claim.PolicyId == policyId, cancellationToken);
    }

    public async Task AddAsync(Policy policy, CancellationToken cancellationToken)
    {
        await _context.Policies.AddAsync(policy, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        _context.Policies.Attach(policy);
        var entry = _context.Entry(policy);
        entry.Property(current => current.PolicyName).IsModified = true;
        entry.Property(current => current.PolicyType).IsModified = true;
        entry.Property(current => current.CoverageAmount).IsModified = true;
        entry.Property(current => current.PremiumAmount).IsModified = true;
        entry.Property(current => current.CustomerId).IsModified = true;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Policy policy, CancellationToken cancellationToken)
    {
        _context.Policies.Remove(policy);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
