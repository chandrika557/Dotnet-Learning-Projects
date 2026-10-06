namespace InsuranceManagement.MVC.Models;

public class Claim // Class to represent an insurance claim (dtos)
{
    public int ClaimId { get; set; }
    public int PolicyId { get; set; }
    public Policy Policy { get; set; } = null!;
    public decimal ClaimAmount { get; set; }
    public DateTime ClaimDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
}
