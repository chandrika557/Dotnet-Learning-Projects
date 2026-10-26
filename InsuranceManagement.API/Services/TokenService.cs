using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.Services;
using InsuranceManagement.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceManagement.API.Services;

/// <summary>Creates short-lived HMAC-signed JWT bearer tokens.</summary>
internal sealed class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _options = options.Value;

    public AuthResponse CreateAccessToken(UserAccount userAccount)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_options.AccessTokenMinutes);
        var claims = new[]
        {
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, userAccount.UserAccountId.ToString()),
            new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userAccount.UserAccountId.ToString()),
            new System.Security.Claims.Claim(ClaimTypes.Email, userAccount.Email),
            new System.Security.Claims.Claim(ClaimTypes.Role, userAccount.Role),
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt,
            userAccount.Role,
            userAccount.UserAccountId);
    }
}
