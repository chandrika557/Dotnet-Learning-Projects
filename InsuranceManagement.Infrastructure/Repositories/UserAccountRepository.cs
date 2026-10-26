using InsuranceManagement.Infrastructure.Data;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.Application.Repositories;
using InsuranceManagement.Application.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.Infrastructure.Repositories;

/// <summary>Implements user-account persistence operations with Entity Framework Core.</summary>
internal sealed class UserAccountRepository(ApplicationDbContext dbContext) : IUserAccountRepository
{
    public Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        dbContext.UserAccounts.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public Task<UserAccount?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.UserAccounts.FirstOrDefaultAsync(user => user.UserAccountId == id, cancellationToken);

    public async Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken)
    {
        await dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            throw new ConflictException("An account with this email already exists.");
        }

        return userAccount;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
