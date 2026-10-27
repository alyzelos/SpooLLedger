using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IFilamentTypeRepository
{
    Task<IReadOnlyList<FilamentType>> GetAllAsync(CancellationToken cancellationToken);
    Task<FilamentType?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<bool> NameTakenAsync(string name, int? excludeId, CancellationToken cancellationToken);
    Task<bool> IsUsedByStockAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(FilamentType type, CancellationToken cancellationToken);
    Task UpdateAsync(FilamentType type, string previousName, CancellationToken cancellationToken);
    Task DeleteAsync(FilamentType type, CancellationToken cancellationToken);
}
