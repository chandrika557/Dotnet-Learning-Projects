using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InsuranceManagement.API.Services;

/// <summary>Implements registration, password verification, and account-role management.</summary>
public sealed class AuthService(
    IUserAccountRepository userRepository,
    IPasswordHasher<UserAccount> passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await userRepository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var account = new UserAccount
        {
            Email = email,
            Role = UserRoles.Customer
        };
        account.PasswordHash = passwordHasher.HashPassword(account, request.Password);
        account = await userRepository.AddAsync(account, cancellationToken);

        return tokenService.CreateAccessToken(account);
    }

    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var account = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (account is null)
        {
            return null;
        }

        var verification = passwordHasher.VerifyHashedPassword(
            account,
            account.PasswordHash,
            request.Password);
        if (verification == PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            account.PasswordHash = passwordHasher.HashPassword(account, request.Password);
            await userRepository.SaveChangesAsync(cancellationToken);
        }

        return tokenService.CreateAccessToken(account);
    }

    public async Task<bool> UpdateRoleAsync(
        int userId,
        string role,
        CancellationToken cancellationToken)
    {
        if (role is not (UserRoles.Customer or UserRoles.ClaimsAdjuster or UserRoles.Administrator))
        {
            throw new ValidationException("Role must be Customer, ClaimsAdjuster, or Administrator.");
        }

        var account = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (account is null)
        {
            return false;
        }

        account.Role = role;
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
