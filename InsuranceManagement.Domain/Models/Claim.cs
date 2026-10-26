namespace InsuranceManagement.Domain.Models;

/// <summary>A claim submitted against an insurance policy.</summary>
public sealed class Claim
{
    /// <summary>Database-generated claim identifier.</summary>
    public int ClaimId { get; set; }

    /// <summary>Identifier of the policy associated with the claim.</summary>
    public int PolicyId { get; set; }

    /// <summary>Policy associated with the claim.</summary>
    public Policy Policy { get; set; } = null!;

    /// <summary>Amount requested by the claimant.</summary>
    public decimal ClaimAmount { get; set; }

    /// <summary>Timestamp when the claim was submitted.</summary>
    public DateTimeOffset ClaimDate { get; set; }

    /// <summary>Explanation of the claimed loss or expense.</summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>Current processing state of the claim.</summary>
    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
}
