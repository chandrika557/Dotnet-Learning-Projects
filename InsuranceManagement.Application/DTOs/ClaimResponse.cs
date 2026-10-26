using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.DTOs;

/// <summary>Claim details returned by the insurance management API.</summary>
/// <param name="ClaimId">Claim identifier.</param>
/// <param name="PolicyId">Identifier of the associated policy.</param>
/// <param name="ClaimAmount">Amount requested by the claimant.</param>
/// <param name="ClaimDate">Submission timestamp with timezone offset.</param>
/// <param name="Reason">Explanation of the claimed loss or expense.</param>
/// <param name="Status">Current processing status.</param>
public sealed record ClaimResponse(
    int ClaimId,
    int PolicyId,
    decimal ClaimAmount,
    DateTimeOffset ClaimDate,
    string Reason,
    ClaimStatus Status);
