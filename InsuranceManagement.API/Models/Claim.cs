namespace InsuranceManagement.API.Models;

/// <summary>A claim submitted against an insurance policy.</summary>
public sealed class Claim
{
    public int ClaimId { get; set; }

    public int PolicyId { get; set; }

    public Policy Policy { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public DateTimeOffset ClaimDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
}
