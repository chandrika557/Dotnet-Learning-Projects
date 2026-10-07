using AutoMapper;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Repositories;

namespace InsuranceManagement.API.Services;

/// <summary>Implements policy business rules and response mapping.</summary>
public sealed class PolicyService(
    IPolicyRepository policyRepository,
    ICustomerRepository customerRepository,
    IMapper mapper) : IPolicyService
{
    public async Task<IReadOnlyList<PolicyResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        mapper.Map<IReadOnlyList<PolicyResponse>>(
            await policyRepository.GetAllAsync(cancellationToken));

    public async Task<PolicyResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var policy = await policyRepository.GetByIdAsync(id, cancellationToken);
        return policy is null ? null : mapper.Map<PolicyResponse>(policy);
    }

    public async Task<PolicyResponse> CreateAsync(
        CreatePolicyRequest request,
        CancellationToken cancellationToken)
    {
        if (await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken) is null)
        {
            throw new ValidationException("The specified customer does not exist.");
        }

        var policy = mapper.Map<Policy>(request);
        policy.PolicyName = policy.PolicyName.Trim();
        return mapper.Map<PolicyResponse>(
            await policyRepository.AddAsync(policy, cancellationToken));
    }

    public async Task<PolicyResponse?> UpdateAsync(
        int id,
        UpdatePolicyRequest request,
        CancellationToken cancellationToken)
    {
        if (await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken) is null)
        {
            throw new ValidationException("The specified customer does not exist.");
        }

        var policy = mapper.Map<Policy>(request);
        policy.PolicyId = id;
        policy.PolicyName = policy.PolicyName.Trim();
        var updated = await policyRepository.UpdateAsync(policy, cancellationToken);
        return updated is null ? null : mapper.Map<PolicyResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (await policyRepository.HasClaimsAsync(id, cancellationToken))
        {
            throw new ConflictException("A policy with existing claims cannot be deleted.");
        }

        return await policyRepository.DeleteAsync(id, cancellationToken);
    }
}
