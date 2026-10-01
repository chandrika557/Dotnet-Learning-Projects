using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.MVC.Models;

public enum PolicyType
{
    [Display(Name = "Health Insurance")]
    Health = 1,
    [Display(Name = "Vehicle Insurance")]
    Vehicle = 2,
    [Display(Name = "Home Insurance")]
    Home = 3,
    [Display(Name = "Travel Insurance")]
    Travel = 4
}
