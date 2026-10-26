using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Services;

/// <summary>Defines account registration, login, and role-management use cases.</summary>
public interface IAuthService
{
    /// <summary>Registers a customer account and issues its access token.</summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    /// <summary>Verifies credentials and issues a token, or returns null for invalid credentials.</summary>
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>Gets safe profile details for an authenticated account.</summary>
    Task<UserProfileResponse?> GetProfileAsync(int userId, CancellationToken cancellationToken);

    /// <summary>Changes an account role and returns false when the account does not exist.</summary>
    Task<bool> UpdateRoleAsync(int userId, string role, CancellationToken cancellationToken);
}
