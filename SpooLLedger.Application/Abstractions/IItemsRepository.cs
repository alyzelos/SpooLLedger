using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IItemsRepository
{
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken);
    Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> IsUsedInOrdersAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Item item, CancellationToken cancellationToken);
    Task UpdateAsync(Item item, CancellationToken cancellationToken);
    Task DeleteAsync(Item item, CancellationToken cancellationToken);
}
