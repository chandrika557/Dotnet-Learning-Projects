namespace InsuranceManagement.API.Models;

/// <summary>Supported application roles used by authorization policies.</summary>
public static class UserRoles
{
    public const string Customer = "Customer";
    public const string ClaimsAdjuster = "ClaimsAdjuster";
    public const string Administrator = "Administrator";
}
