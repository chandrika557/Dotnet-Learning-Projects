namespace InsuranceManagement.API.DTOs;

/// <summary>Credentials used to obtain a JSON Web Token.</summary>
public sealed record LoginRequest
{
    /// <summary>Registered email address.</summary>
    public required string Email { get; init; }

    /// <summary>Account password.</summary>
    public required string Password { get; init; }
}
