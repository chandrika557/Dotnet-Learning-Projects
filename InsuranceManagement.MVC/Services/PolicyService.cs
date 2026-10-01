using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.Repositories;

namespace InsuranceManagement.MVC.Services;

public sealed class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _policyRepository;
    private readonly ICustomerRepository _customerRepository;

    public PolicyService(
        IPolicyRepository policyRepository,
        ICustomerRepository customerRepository)
    {
        _policyRepository = policyRepository;
        _customerRepository = customerRepository;
    }

    public Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _policyRepository.GetAllAsync(cancellationToken);
    }

    public Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _policyRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> CreateAsync(Policy policy, CancellationToken cancellationToken)
    {
        if (await _customerRepository.GetByIdAsync(policy.CustomerId, cancellationToken) is null)
        {
            return false;
        }

        Normalize(policy);
        await _policyRepository.AddAsync(policy, cancellationToken);
        return true;
    }

    public Task<bool> HasClaimsAsync(int policyId, CancellationToken cancellationToken)
    {
        return _policyRepository.HasClaimsAsync(policyId, cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        int id,
        Policy changes,
        CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
        if (policy is null ||
            await _customerRepository.GetByIdAsync(changes.CustomerId, cancellationToken) is null)
        {
            return false;
        }

        if (policy.CustomerId != changes.CustomerId &&
            await _policyRepository.HasClaimsAsync(id, cancellationToken))
        {
            return false;
        }

        policy.PolicyName = changes.PolicyName;
        policy.PolicyType = changes.PolicyType;
        policy.CoverageAmount = changes.CoverageAmount;
        policy.PremiumAmount = changes.PremiumAmount;
        policy.CustomerId = changes.CustomerId;
        Normalize(policy);

        await _policyRepository.UpdateAsync(policy, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);
        if (policy is null ||
            await _policyRepository.HasClaimsAsync(id, cancellationToken))
        {
            return false;
        }

        await _policyRepository.DeleteAsync(policy, cancellationToken);
        return true;
    }

    private static void Normalize(Policy policy)
    {
        policy.PolicyName = policy.PolicyName.Trim();
    }
}
