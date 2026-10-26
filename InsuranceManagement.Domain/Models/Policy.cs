namespace InsuranceManagement.Domain.Models;

/// <summary>Insurance coverage owned by a customer.</summary>
public sealed class Policy
{
    /// <summary>Database-generated policy identifier.</summary>
    public int PolicyId { get; set; }

    /// <summary>Display name of the coverage.</summary>
    public string PolicyName { get; set; } = string.Empty;

    /// <summary>Insurance category covered by the policy.</summary>
    public PolicyType PolicyType { get; set; }

    /// <summary>Maximum amount covered by the policy.</summary>
    public decimal CoverageAmount { get; set; }

    /// <summary>Premium charged for the policy.</summary>
    public decimal PremiumAmount { get; set; }

    /// <summary>Identifier of the customer who owns the policy.</summary>
    public int CustomerId { get; set; }

    /// <summary>Customer who owns the policy.</summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>Claims filed against this policy.</summary>
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
