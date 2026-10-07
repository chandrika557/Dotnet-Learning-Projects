using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Services;

/// <summary>Issues signed access tokens for authenticated accounts.</summary>
public interface ITokenService
{
    AuthResponse CreateAccessToken(UserAccount userAccount);
}
