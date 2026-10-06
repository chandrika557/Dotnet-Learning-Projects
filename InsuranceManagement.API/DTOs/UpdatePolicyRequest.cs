using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for replacing policy details.</summary>
public sealed record UpdatePolicyRequest
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
