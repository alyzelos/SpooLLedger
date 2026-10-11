namespace SpooLLedger.Application.Abstractions;

public interface IFilamentStockRepository
{
    Task<IReadOnlyList<global::SpooLLedger.Domain.FilamentStock>> GetAllAsync(CancellationToken cancellationToken);
    Task<global::SpooLLedger.Domain.FilamentStock?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(global::SpooLLedger.Domain.FilamentStock stock, CancellationToken cancellationToken);
    Task UpdateAsync(global::SpooLLedger.Domain.FilamentStock stock, CancellationToken cancellationToken);
    Task DeleteAsync(global::SpooLLedger.Domain.FilamentStock stock, CancellationToken cancellationToken);
}
