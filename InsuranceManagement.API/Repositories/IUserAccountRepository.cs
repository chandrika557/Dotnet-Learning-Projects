using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Repositories;

/// <summary>Defines persistence operations for registered API accounts.</summary>
public interface IUserAccountRepository
{
    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserAccount?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
