using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.DTOs;

/// <summary>Policy details returned by the insurance management API.</summary>
/// <param name="PolicyId">Policy identifier.</param>
/// <param name="PolicyName">Display name for the insurance policy.</param>
/// <param name="PolicyType">Insurance category.</param>
/// <param name="CoverageAmount">Maximum amount covered by the policy.</param>
/// <param name="PremiumAmount">Premium charged for the policy.</param>
/// <param name="CustomerId">Identifier of the owning customer.</param>
public sealed record PolicyResponse(
    int PolicyId,
    string PolicyName,
    PolicyType PolicyType,
    decimal CoverageAmount,
    decimal PremiumAmount,
    int CustomerId);
