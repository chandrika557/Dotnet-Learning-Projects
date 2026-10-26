namespace InsuranceManagement.Domain.Models;

/// <summary>Categories of insurance policies offered to customers.</summary>
public enum PolicyType
{
    /// <summary>Health insurance coverage.</summary>
    Health = 1,
    /// <summary>Vehicle insurance coverage.</summary>
    Vehicle = 2,
    /// <summary>Home insurance coverage.</summary>
    Home = 3,
    /// <summary>Travel insurance coverage.</summary>
    Travel = 4
}
