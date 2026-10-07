namespace InsuranceManagement.API.DTOs;

/// <summary>Credentials used to register a customer account.</summary>
public sealed record RegisterRequest
{
    /// <summary>Email address used as the account name.</summary>
    public required string Email { get; init; }

    /// <summary>Plaintext password sent over TLS; the server stores only a password hash.</summary>
    public required string Password { get; init; }
}
