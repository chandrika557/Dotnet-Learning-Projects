namespace InsuranceManagement.Domain.Models;

/// <summary>A registered API user with a securely hashed password and assigned role.</summary>
public sealed class UserAccount
{
    /// <summary>Database-generated account identifier.</summary>
    public int UserAccountId { get; set; }

    /// <summary>Unique email address used for login.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>One-way password hash; plaintext passwords are never stored.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Authorization role assigned to the account.</summary>
    public string Role { get; set; } = UserRoles.Customer;
}
