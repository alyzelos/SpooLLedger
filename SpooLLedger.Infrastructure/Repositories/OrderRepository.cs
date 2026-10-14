using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class OrderRepository(DataContext dataContext) : IOrderRspository
{
    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken) => 
        await dataContext.Orders
            .AsNoTracking()
            .Include(order => order.Customer)
            .ToListAsync(cancellationToken);

    public async Task<Order> GetByOrderNumber(int orderNumber, CancellationToken cancellationToken)
    {
        var order =  await dataContext.Orders
            .AsNoTracking()
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(o=> o.OrderNumber == orderNumber);
        if(order == null)
        {
            throw new Exception($"Order with {orderNumber} was not found");
        }
        return order;
    }
    public async Task<Order> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

        var lastNumber = await dataContext.Orders
            .MaxAsync(order => (int?)order.OrderNumber, cancellationToken) ?? 0;

        var order = new Order
        {
            OrderNumber = lastNumber + 1,
            OrderDate = command.OrderDate,
            Status = command.Status,
            CustomerId = command.CustomerId,
        };
        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return order;
    }
}
