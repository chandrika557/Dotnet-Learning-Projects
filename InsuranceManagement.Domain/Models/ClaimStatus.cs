namespace InsuranceManagement.Domain.Models;

/// <summary>Processing state of an insurance claim.</summary>
public enum ClaimStatus
{
    /// <summary>Submitted and awaiting review.</summary>
    Pending = 1,
    /// <summary>Approved by a claims adjuster.</summary>
    Approved = 2,
    /// <summary>Rejected by a claims adjuster.</summary>
    Rejected = 3
}
