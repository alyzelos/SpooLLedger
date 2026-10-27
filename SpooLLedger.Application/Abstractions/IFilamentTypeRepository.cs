using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IFilamentTypeRepository
{
    Task<IReadOnlyList<FilamentType>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
}
