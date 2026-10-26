using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Services;

/// <summary>Issues signed access tokens for authenticated accounts.</summary>
public interface ITokenService
{
    /// <summary>Creates a signed access token for an account.</summary>
    AuthResponse CreateAccessToken(UserAccount userAccount);
}
