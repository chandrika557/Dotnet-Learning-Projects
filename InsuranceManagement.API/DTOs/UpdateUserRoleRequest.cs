namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for an administrator to change a user's role.</summary>
public sealed record UpdateUserRoleRequest
{
    /// <summary>One of Customer, ClaimsAdjuster, or Administrator.</summary>
    public required string Role { get; init; }
}
