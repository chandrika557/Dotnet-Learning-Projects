using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.DTOs;

/// <summary>Claim details returned by the insurance management API.</summary>
public sealed record ClaimResponse(
    int ClaimId,
    int PolicyId,
    decimal ClaimAmount,
    DateTimeOffset ClaimDate,
    string Reason,
    ClaimStatus Status);
