using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class OrderRepository(DataContext dataContext) : IOrderRepository
{
    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken) => 
        await dataContext.Orders
            .AsNoTracking()
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .ThenInclude(line => line.Item)
            .ToListAsync(cancellationToken);

    public async Task<Order> GetByOrderNumber(int orderNumber, CancellationToken cancellationToken)
    {
        var order =  await dataContext.Orders
            .AsNoTracking()
            .Include(c => c.Customer)
            .Include(order => order.Items)
            .ThenInclude(line => line.Item)
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
            Items = await BuildLines(command.Items, cancellationToken)
        };
        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await GetByOrderNumber(order.OrderNumber, cancellationToken);
    }
    public async Task<Order> UpdateOrder(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);
        
        var orderToUpdate = await dataContext.Orders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == command.OrderNumber, cancellationToken);
        if (orderToUpdate is null) throw new Exception("order is not found");

        orderToUpdate.OrderDate = command.OrderDate;
        orderToUpdate.Status = command.Status ?? orderToUpdate.Status;
        orderToUpdate.CustomerId = command.CustomerId;
        dataContext.OrderItems.RemoveRange(orderToUpdate.Items);
        orderToUpdate.Items = await BuildLines(command.Items, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await GetByOrderNumber(orderToUpdate.OrderNumber, cancellationToken);
    }

    private async Task<List<OrderItem>> BuildLines(IReadOnlyList<OrderItemRequest> lines, CancellationToken cancellationToken)
    {
        var itemIds = lines.Select(line => line.ItemId).Distinct().ToList();
        var existingIds = await dataContext.Items
            .Where(item => itemIds.Contains(item.Id))
            .Select(item => item.Id)
            .ToListAsync(cancellationToken);

        var missingId = itemIds.Except(existingIds).FirstOrDefault();
        if (missingId != 0)
        {
            throw new Exception($"Item {missingId} was not found");
        }

        return lines.Select(line => new OrderItem
        {
            ItemId = line.ItemId,
            Quantity = line.Quantity
        }).ToList();
    }

}
