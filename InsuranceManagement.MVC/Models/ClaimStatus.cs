using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.MVC.Models;

public enum ClaimStatus // Enum to represent the status of a claim
{
    [Display(Name = "Pending")] // Display attribute to specify the display name for the enum value
    Pending = 1,
    [Display(Name = "Approved")]
    Approved = 2,
    [Display(Name = "Rejected")]
    Rejected = 3
}
