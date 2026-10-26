namespace InsuranceManagement.Domain.Models;

/// <summary>Supported application roles used by authorization policies.</summary>
public static class UserRoles
{
    /// <summary>Regular account role assigned during public registration.</summary>
    public const string Customer = "Customer";
    /// <summary>May manage insurance customers, policies, and claims.</summary>
    public const string ClaimsAdjuster = "ClaimsAdjuster";
    /// <summary>May perform all operations, including role provisioning.</summary>
    public const string Administrator = "Administrator";
}
