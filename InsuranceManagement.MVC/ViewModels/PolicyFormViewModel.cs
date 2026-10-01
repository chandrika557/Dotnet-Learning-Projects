using System.ComponentModel.DataAnnotations;
using InsuranceManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class PolicyFormViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Policy name")]
    public string PolicyName { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(PolicyType))]
    [Display(Name = "Policy type")]
    public PolicyType? PolicyType { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    public IReadOnlyList<SelectListItem> CustomerOptions { get; set; } = [];
    public bool CanChangeCustomer { get; set; } = true;

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    [Display(Name = "Coverage amount")]
    public decimal CoverageAmount { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    [Display(Name = "Premium amount")]
    public decimal PremiumAmount { get; set; }
}
