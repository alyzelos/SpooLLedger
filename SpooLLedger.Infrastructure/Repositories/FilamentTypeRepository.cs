using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class FilamentTypeRepository(DataContext context) : IFilamentTypeRepository
{
    public async Task<IReadOnlyList<FilamentType>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.FilamentTypes.AsNoTracking().OrderBy(t => t.Name).ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentTypes.AnyAsync(t => t.Name == name, cancellationToken);
}
