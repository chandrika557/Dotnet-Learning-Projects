using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class ClaimListItemViewModel
{
    public int ClaimId { get; init; }
    public int PolicyId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string PolicyName { get; init; } = string.Empty;
    public decimal ClaimAmount { get; init; }
    public DateTime ClaimDate { get; init; }
    public string Reason { get; init; } = string.Empty;
    public ClaimStatus Status { get; init; }
}
