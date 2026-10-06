using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for issuing a policy to a customer.</summary>
public sealed record CreatePolicyRequest
{
    /// <summary>Display name for the insurance policy.</summary>
    public required string PolicyName { get; init; }

    /// <summary>Insurance category for the policy.</summary>
    public required PolicyType PolicyType { get; init; }

    /// <summary>Maximum amount covered by the policy.</summary>
    public decimal CoverageAmount { get; init; }

    /// <summary>Premium charged for the policy.</summary>
    public decimal PremiumAmount { get; init; }

    /// <summary>Identifier of the customer who owns this policy.</summary>
    public int CustomerId { get; init; }
}
