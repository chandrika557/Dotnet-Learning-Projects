namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for submitting a claim against a policy.</summary>
public sealed record CreateClaimRequest
{
    /// <summary>Policy identifier that the claim refers to.</summary>
    public int PolicyId { get; init; }

    /// <summary>Amount requested for the claim.</summary>
    public decimal ClaimAmount { get; init; }

    /// <summary>Reason describing the loss or expense.</summary>
    public required string Reason { get; init; }
}
