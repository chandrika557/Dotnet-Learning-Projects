using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class PolicyListItemViewModel
{
    public int PolicyId { get; init; }
    public string PolicyName { get; init; } = string.Empty;
    public PolicyType PolicyType { get; init; }
    public decimal CoverageAmount { get; init; }
    public decimal PremiumAmount { get; init; }
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
}
