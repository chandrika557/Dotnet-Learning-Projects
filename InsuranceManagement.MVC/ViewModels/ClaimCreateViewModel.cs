using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsuranceManagement.MVC.ViewModels;

public sealed class ClaimCreateViewModel
{
    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name = "Customer policy")]
    public int PolicyId { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    [Display(Name = "Claim amount")]
    public decimal ClaimAmount { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public IReadOnlyList<SelectListItem> PolicyOptions { get; set; } = [];
}
