using Microsoft.Extensions.DependencyInjection;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Repositories;

namespace SpooLLedger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFilamentStockRepository, FilamentStockRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}
