namespace InsuranceManagement.Application.DTOs;

/// <summary>Access token and role information returned after authentication.</summary>
/// <param name="AccessToken">Signed bearer token that must be kept confidential by the client.</param>
/// <param name="ExpiresAt">UTC expiry instant for the access token.</param>
/// <param name="Role">Role assigned to the authenticated account.</param>
/// <param name="UserId">Identifier of the authenticated account.</param>
public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    string Role,
    int UserId);
