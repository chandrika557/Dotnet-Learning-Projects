using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class CustomerFormViewModel // ViewModel class for customer form
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Customer name")] // Display attribute to specify the display name for the property
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
