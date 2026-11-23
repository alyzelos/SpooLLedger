using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IFilamentColorRepository
{
    Task<IReadOnlyList<FilamentColor>> GetAllAsync(CancellationToken cancellationToken);
    Task<FilamentColor?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<bool> NameTakenAsync(string name, int? excludeId, CancellationToken cancellationToken);
    Task<bool> IsUsedByStockAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(FilamentColor color, CancellationToken cancellationToken);
    Task UpdateAsync(FilamentColor color, string previousName, CancellationToken cancellationToken);
    Task DeleteAsync(FilamentColor color, CancellationToken cancellationToken);
}
