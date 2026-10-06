namespace InsuranceManagement.API.Models;

/// <summary>Insurance coverage owned by a customer.</summary>
public sealed class Policy
{
    public int PolicyId { get; set; }

    public string PolicyName { get; set; } = string.Empty;

    public PolicyType PolicyType { get; set; }

    public decimal CoverageAmount { get; set; }

    public decimal PremiumAmount { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
