using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Repositories;

/// <summary>Defines persistence operations for registered API accounts.</summary>
public interface IUserAccountRepository
{
    /// <summary>Finds an account by its normalized email address.</summary>
    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>Finds an account by its identifier.</summary>
    Task<UserAccount?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>Adds an account and persists it.</summary>
    Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken);

    /// <summary>Persists pending account changes.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
