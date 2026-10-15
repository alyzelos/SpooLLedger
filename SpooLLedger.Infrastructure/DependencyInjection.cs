using Microsoft.Extensions.DependencyInjection;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Infrastructure.Repositories;

namespace SpooLLedger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFilamentStockRepository, FilamentStockRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
