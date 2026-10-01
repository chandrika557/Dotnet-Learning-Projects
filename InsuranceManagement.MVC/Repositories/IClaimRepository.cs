using InsuranceManagement.MVC.Models;

namespace InsuranceManagement.MVC.Repositories;

public interface IClaimRepository
{
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken);
    Task<Claim?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Claim claim, CancellationToken cancellationToken);
    Task UpdateAsync(Claim claim, CancellationToken cancellationToken);
}
