using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for an adjuster to approve or reject a claim.</summary>
public sealed record UpdateClaimStatusRequest
{
    /// <summary>New processing status for the claim.</summary>
    public ClaimStatus Status { get; init; }
}
