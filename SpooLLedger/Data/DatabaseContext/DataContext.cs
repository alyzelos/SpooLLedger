using System;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Entities;

namespace SpooLLedger.Data.DatabaseContext;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FilamentStock> FilamentStocks { get; set; }
}
