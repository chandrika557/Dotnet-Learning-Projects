using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class CustomerFormViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Customer name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }
}
