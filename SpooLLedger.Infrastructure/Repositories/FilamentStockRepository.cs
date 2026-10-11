using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class FilamentStockRepository(DataContext context) : IFilamentStockRepository
{
    public async Task<IReadOnlyList<FilamentStock>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.FilamentStocks.AsNoTracking().ToListAsync(cancellationToken);

    public Task<FilamentStock?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.FilamentStocks.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task AddAsync(FilamentStock stock, CancellationToken cancellationToken)
    {
        context.FilamentStocks.Add(stock);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FilamentStock stock, CancellationToken cancellationToken)
    {
        context.FilamentStocks.Update(stock);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FilamentStock stock, CancellationToken cancellationToken)
    {
        context.FilamentStocks.Remove(stock);
        await context.SaveChangesAsync(cancellationToken);
    }
}
