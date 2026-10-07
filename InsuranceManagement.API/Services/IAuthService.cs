using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Services;

/// <summary>Defines account registration, login, and role-management use cases.</summary>
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateRoleAsync(int userId, string role, CancellationToken cancellationToken);
}
