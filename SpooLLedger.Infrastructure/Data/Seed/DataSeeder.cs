using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Domain;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Data.Seed;

public class DataSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task SeedFilamentTypes(DataContext dataContext)
    {
        if (await dataContext.FilamentTypes.AnyAsync()) return;

        var types = await ReadSeed<FilamentType>("FilamentType.json");
        if (types.Count == 0) return;

        dataContext.FilamentTypes.AddRange(types);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedPricing(DataContext dataContext)
    {
        if (await dataContext.Pricings.AnyAsync()) return;

        var pricings = await ReadSeed<Pricing>("Pricing.json");
        if (pricings.Count == 0) return;

        dataContext.Pricings.AddRange(pricings);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedFilamentStock(DataContext dataContext)
    {
        if (await dataContext.FilamentStocks.AnyAsync()) return;

        var filaments = await ReadSeed<FilamentStock>("FilamentStock.json");
        if (filaments.Count == 0) return;

        dataContext.FilamentStocks.AddRange(filaments);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedCustomers(DataContext dataContext)
    {
        if (await dataContext.Customers.AnyAsync()) return;

        var customers = await ReadSeed<Customer>("Customer.json");
        if (customers.Count == 0) return;

        dataContext.Customers.AddRange(customers);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedOrders(DataContext dataContext)
    {
        if (await dataContext.Orders.AnyAsync()) return;

        var orders = await ReadSeed<Order>("Order.json");
        if (orders.Count == 0) return;

        dataContext.Orders.AddRange(orders);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedItems(DataContext dataContext)
    {
        if (await dataContext.Items.AnyAsync()) return;

        var items = await ReadSeed<Item>("Item.json");
        if (items.Count == 0) return;

        dataContext.Items.AddRange(items);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedOrderItems(DataContext dataContext)
    {
        if (await dataContext.OrderItems.AnyAsync()) return;

        var items = await ReadSeed<OrderItem>("OrderItem.json");
        if (items.Count == 0) return;

        dataContext.OrderItems.AddRange(items);
        await dataContext.SaveChangesAsync();
    }

    private static async Task<List<T>> ReadSeed<T>(string fileName)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName));
        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? [];
    }
}
