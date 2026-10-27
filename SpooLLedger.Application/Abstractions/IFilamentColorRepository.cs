using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IFilamentColorRepository
{
    Task<IReadOnlyList<FilamentColor>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
}
