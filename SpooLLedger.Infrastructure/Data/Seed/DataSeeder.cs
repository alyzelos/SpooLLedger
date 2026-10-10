using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Domain;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Data.Seed;

public class DataSeeder
{
    public static async Task SeedFilamentStock(DataContext dataContext)
    {
        if (await dataContext.FilamentStocks.AnyAsync()) return;

        var filamentData = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "FilamentStock.json"));

        var option = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var filaments = JsonSerializer.Deserialize<List<FilamentStock>>(filamentData, option);

        if (filaments == null) return;
        foreach (var filament in filaments)
        {
            dataContext.FilamentStocks.Add(filament);
        }
        await dataContext.SaveChangesAsync();
    }
}
