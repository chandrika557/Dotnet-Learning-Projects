using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.API.Services;

/// <summary>Configuration required to issue and validate signed JWT access tokens.</summary>
public sealed class JwtOptions
{
    /// <summary>Configuration section containing the JWT settings.</summary>
    public const string SectionName = "Jwt";

    /// <summary>Expected issuer for access tokens.</summary>
    [Required]
    public string Issuer { get; init; } = string.Empty;

    /// <summary>Expected audience for access tokens.</summary>
    [Required]
    public string Audience { get; init; } = string.Empty;

    /// <summary>Secret HMAC signing key, supplied via user secrets or environment.</summary>
    [Required, MinLength(32)]
    public string SigningKey { get; init; } = string.Empty;

    /// <summary>Access token lifetime in minutes.</summary>
    [Range(1, 1440)]
    public int AccessTokenMinutes { get; init; } = 30;
}
