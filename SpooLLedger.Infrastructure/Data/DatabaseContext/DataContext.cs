using Microsoft.EntityFrameworkCore;
using SpooLLedger.Domain;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.DatabaseContext;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FilamentStock> FilamentStocks { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
