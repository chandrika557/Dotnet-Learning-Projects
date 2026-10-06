namespace InsuranceManagement.API.DTOs;

/// <summary>Access token and role information returned after authentication.</summary>
public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    string Role);
