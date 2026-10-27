using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class ItemsRepository(DataContext dataContext) : IItemsRepository
{
    public async Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken) =>
        await dataContext.Items.AsNoTracking().OrderBy(i => i.ProductName).ToListAsync(cancellationToken);

    public Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        dataContext.Items.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public Task<bool> IsUsedInOrdersAsync(int id, CancellationToken cancellationToken) =>
        dataContext.OrderItems.AnyAsync(line => line.ItemId == id, cancellationToken);

    public async Task AddAsync(Item item, CancellationToken cancellationToken)
    {
        dataContext.Items.Add(item);
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
    {
        dataContext.Items.Update(item);
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Item item, CancellationToken cancellationToken)
    {
        dataContext.Items.Remove(item);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
