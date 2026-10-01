using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.MVC.Models;

public enum ClaimStatus
{
    [Display(Name = "Pending")]
    Pending = 1,
    [Display(Name = "Approved")]
    Approved = 2,
    [Display(Name = "Rejected")]
    Rejected = 3
}
