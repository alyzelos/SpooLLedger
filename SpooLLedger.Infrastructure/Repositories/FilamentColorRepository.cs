using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class FilamentColorRepository(DataContext context) : IFilamentColorRepository
{
    public async Task<IReadOnlyList<FilamentColor>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.FilamentColors.AsNoTracking().OrderBy(color => color.Name).ToListAsync(cancellationToken);

    public Task<FilamentColor?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.FilamentColors.FirstOrDefaultAsync(color => color.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentColors.AnyAsync(color => color.Name == name, cancellationToken);

    public Task<bool> NameTakenAsync(string name, int? excludeId, CancellationToken cancellationToken) =>
        context.FilamentColors.AnyAsync(color => color.Name == name && color.Id != excludeId, cancellationToken);

    public Task<bool> IsUsedByStockAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentStocks.AnyAsync(stock => stock.Color == name, cancellationToken);

    public async Task AddAsync(FilamentColor color, CancellationToken cancellationToken)
    {
        context.FilamentColors.Add(color);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FilamentColor color, string previousName, CancellationToken cancellationToken)
    {
        if (previousName != color.Name)
        {
            var stocks = await context.FilamentStocks
                .Where(stock => stock.Color == previousName)
                .ToListAsync(cancellationToken);
            foreach (var stock in stocks)
                stock.Color = color.Name;
        }

        context.FilamentColors.Update(color);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FilamentColor color, CancellationToken cancellationToken)
    {
        context.FilamentColors.Remove(color);
        await context.SaveChangesAsync(cancellationToken);
    }
}
