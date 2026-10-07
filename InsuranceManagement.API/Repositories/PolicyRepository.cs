using InsuranceManagement.API.Data;
using InsuranceManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.API.Repositories;

/// <summary>Implements policy persistence operations with Entity Framework Core.</summary>
public sealed class PolicyRepository(ApplicationDbContext dbContext) : IPolicyRepository
{
    public async Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Policies.AsNoTracking()
            .OrderBy(policy => policy.PolicyName)
            .ThenBy(policy => policy.PolicyId)
            .ToListAsync(cancellationToken);

    public Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Policies.AsNoTracking()
            .FirstOrDefaultAsync(policy => policy.PolicyId == id, cancellationToken);

    public Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken) =>
        dbContext.Claims.AnyAsync(claim => claim.PolicyId == policyId, cancellationToken);

    public async Task<Policy> AddAsync(Policy policy, CancellationToken cancellationToken)
    {
        await dbContext.Policies.AddAsync(policy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return policy;
    }

    public async Task<Policy?> UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Policies.FirstOrDefaultAsync(
            item => item.PolicyId == policy.PolicyId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.PolicyName = policy.PolicyName;
        existing.PolicyType = policy.PolicyType;
        existing.CoverageAmount = policy.CoverageAmount;
        existing.PremiumAmount = policy.PremiumAmount;
        existing.CustomerId = policy.CustomerId;
        await dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var policy = await dbContext.Policies.FirstOrDefaultAsync(
            item => item.PolicyId == id, cancellationToken);
        if (policy is null)
        {
            return false;
        }

        dbContext.Policies.Remove(policy);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
