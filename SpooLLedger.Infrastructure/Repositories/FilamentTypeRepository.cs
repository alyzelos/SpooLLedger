using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class FilamentTypeRepository(DataContext context) : IFilamentTypeRepository
{
    public async Task<IReadOnlyList<FilamentType>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.FilamentTypes.AsNoTracking().OrderBy(t => t.Name).ToListAsync(cancellationToken);

    public Task<FilamentType?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.FilamentTypes.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentTypes.AnyAsync(t => t.Name == name, cancellationToken);

    public Task<bool> NameTakenAsync(string name, int? excludeId, CancellationToken cancellationToken) =>
        context.FilamentTypes.AnyAsync(t => t.Name == name && t.Id != excludeId, cancellationToken);

    public Task<bool> IsUsedByStockAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentStocks.AnyAsync(s => s.Type == name, cancellationToken);

    public async Task AddAsync(FilamentType type, CancellationToken cancellationToken)
    {
        context.FilamentTypes.Add(type);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FilamentType type, string previousName, CancellationToken cancellationToken)
    {
        if (previousName != type.Name)
        {
            var stocks = await context.FilamentStocks
                .Where(s => s.Type == previousName)
                .ToListAsync(cancellationToken);
            foreach (var stock in stocks)
                stock.Type = type.Name;
        }

        context.FilamentTypes.Update(type);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FilamentType type, CancellationToken cancellationToken)
    {
        context.FilamentTypes.Remove(type);
        await context.SaveChangesAsync(cancellationToken);
    }
}
