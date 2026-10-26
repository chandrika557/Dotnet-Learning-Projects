namespace InsuranceManagement.Application.DTOs;

/// <summary>Safe account details returned for the authenticated user.</summary>
/// <param name="UserId">Account identifier.</param>
/// <param name="Email">Registered email address.</param>
/// <param name="Role">Assigned application role.</param>
public sealed record UserProfileResponse(int UserId, string Email, string Role);
