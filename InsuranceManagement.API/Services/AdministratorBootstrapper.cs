using System.ComponentModel.DataAnnotations;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InsuranceManagement.API.Services;

/// <summary>Creates the initial administrator only when explicit bootstrap credentials are configured.</summary>
internal static class AdministratorBootstrapper
{
    public static async Task CreateIfConfiguredAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var email = configuration["BootstrapAdministrator:Email"];
        var password = configuration["BootstrapAdministrator:Password"];

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Both BootstrapAdministrator:Email and BootstrapAdministrator:Password must be configured together.");
        }

        if (!new EmailAddressAttribute().IsValid(email)
            || password.Length < 12)
        {
            throw new InvalidOperationException(
                "Bootstrap administrator configuration requires a valid email and a password of at least 12 characters.");
        }

        await using var scope = services.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserAccountRepository>();
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var existingAccount = await repository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingAccount is not null)
        {
            if (existingAccount.Role != UserRoles.Administrator)
            {
                throw new InvalidOperationException(
                    "The configured bootstrap administrator email belongs to a non-administrator account.");
            }

            return;
        }

        var account = new UserAccount
        {
            Email = normalizedEmail,
            Role = UserRoles.Administrator
        };
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UserAccount>>();
        account.PasswordHash = passwordHasher.HashPassword(account, password);
        await repository.AddAsync(account, cancellationToken);
    }
}
