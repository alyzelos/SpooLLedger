using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IPricingRepository
{
    Task<IReadOnlyList<Pricing>> GetAllAsync(CancellationToken cancellationToken);
    Task<Pricing?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Pricing?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken);
    Task AddAsync(Pricing pricing, CancellationToken cancellationToken);
    Task UpdateAsync(Pricing pricing, CancellationToken cancellationToken);
    Task DeleteAsync(Pricing pricing, CancellationToken cancellationToken);
}
