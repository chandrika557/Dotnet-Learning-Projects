using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.DTOs;

/// <summary>Policy details returned by the insurance management API.</summary>
public sealed record PolicyResponse(
    int PolicyId,
    string PolicyName,
    PolicyType PolicyType,
    decimal CoverageAmount,
    decimal PremiumAmount,
    int CustomerId);
