using AutoMapper;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Repositories;

namespace InsuranceManagement.API.Services;

/// <summary>Implements claim business rules and response mapping.</summary>
public sealed class ClaimService(
    IClaimRepository claimRepository,
    IPolicyRepository policyRepository,
    IMapper mapper) : IClaimService
{
    public async Task<IReadOnlyList<ClaimResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        mapper.Map<IReadOnlyList<ClaimResponse>>(
            await claimRepository.GetAllAsync(cancellationToken));

    public async Task<ClaimResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var claim = await claimRepository.GetByIdAsync(id, cancellationToken);
        return claim is null ? null : mapper.Map<ClaimResponse>(claim);
    }

    public async Task<ClaimResponse> CreateAsync(
        CreateClaimRequest request,
        CancellationToken cancellationToken)
    {
        if (await policyRepository.GetByIdAsync(request.PolicyId, cancellationToken) is null)
        {
            throw new ValidationException("The specified policy does not exist.");
        }

        var claim = mapper.Map<Claim>(request);
        claim.ClaimDate = DateTimeOffset.UtcNow;
        claim.Status = ClaimStatus.Pending;
        return mapper.Map<ClaimResponse>(
            await claimRepository.AddAsync(claim, cancellationToken));
    }

    public async Task<ClaimResponse?> UpdateStatusAsync(
        int id,
        UpdateClaimStatusRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Status == ClaimStatus.Pending
            || !Enum.IsDefined(request.Status))
        {
            throw new ValidationException("A claim can only be approved or rejected.");
        }

        var claim = await claimRepository.GetByIdAsync(id, cancellationToken);
        if (claim is null)
        {
            return null;
        }

        if (claim.Status != ClaimStatus.Pending)
        {
            throw new ConflictException("Only pending claims can be reviewed.");
        }

        var updated = await claimRepository.UpdateStatusAsync(id, request.Status, cancellationToken);
        return updated is null ? null : mapper.Map<ClaimResponse>(updated);
    }
}
