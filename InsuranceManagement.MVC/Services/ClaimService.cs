using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.Repositories;

namespace InsuranceManagement.MVC.Services;

public sealed class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimRepository;
    private readonly IPolicyRepository _policyRepository;

    public ClaimService(IClaimRepository claimRepository, IPolicyRepository policyRepository)
    {
        _claimRepository = claimRepository;
        _policyRepository = policyRepository;
    }

    public Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _claimRepository.GetAllAsync(cancellationToken);
    }

    public async Task<ClaimCreationResult> CreateAsync(
        Claim claim,
        CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(claim.PolicyId, cancellationToken);
        if (policy is null)
        {
            return ClaimCreationResult.PolicyNotFound;
        }

        if (claim.ClaimAmount > policy.CoverageAmount)
        {
            return ClaimCreationResult.AmountExceedsCoverage;
        }

        claim.ClaimDate = DateTime.UtcNow;
        claim.Status = ClaimStatus.Pending;
        claim.Reason = claim.Reason.Trim();

        await _claimRepository.AddAsync(claim, cancellationToken);
        return ClaimCreationResult.Created;
    }

    public async Task<bool> SetStatusAsync(
        int id,
        ClaimStatus status,
        CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(id, cancellationToken);
        if (claim is null || claim.Status != ClaimStatus.Pending)
        {
            return false;
        }

        claim.Status = status;
        await _claimRepository.UpdateAsync(claim, cancellationToken);
        return true;
    }
}
