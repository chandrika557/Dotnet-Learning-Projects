namespace InsuranceManagement.API.Models;

/// <summary>A registered API user with a securely hashed password and assigned role.</summary>
public sealed class UserAccount
{
    public int UserAccountId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = UserRoles.Customer;
}
