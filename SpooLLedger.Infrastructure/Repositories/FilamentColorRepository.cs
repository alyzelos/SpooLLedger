using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class FilamentColorRepository(DataContext context) : IFilamentColorRepository
{
    public async Task<IReadOnlyList<FilamentColor>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.FilamentColors.AsNoTracking().OrderBy(color => color.Name).ToListAsync(cancellationToken);

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken) =>
        context.FilamentColors.AnyAsync(color => color.Name == name, cancellationToken);
}
