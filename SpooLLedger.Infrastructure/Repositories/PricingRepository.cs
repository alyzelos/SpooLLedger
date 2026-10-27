using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class PricingRepository(DataContext context) : IPricingRepository
{
    public async Task<IReadOnlyList<Pricing>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.Pricings.AsNoTracking().OrderBy(p => p.Name).ToListAsync(cancellationToken);

    public Task<Pricing?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.Pricings.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken cancellationToken) =>
        context.Pricings.AnyAsync(p => p.Name == name && p.Id != excludeId, cancellationToken);

    public async Task AddAsync(Pricing pricing, CancellationToken cancellationToken)
    {
        context.Pricings.Add(pricing);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Pricing pricing, CancellationToken cancellationToken)
    {
        context.Pricings.Update(pricing);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Pricing pricing, CancellationToken cancellationToken)
    {
        context.Pricings.Remove(pricing);
        await context.SaveChangesAsync(cancellationToken);
    }
}
