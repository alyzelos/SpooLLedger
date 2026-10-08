using Microsoft.EntityFrameworkCore;
using SpooLLedger.Domain;

namespace SpooLLedger.Infrastructure.Data.DatabaseContext;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FilamentStock> FilamentStocks { get; set; }
}
