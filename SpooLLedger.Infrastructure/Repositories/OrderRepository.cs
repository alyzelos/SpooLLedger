using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order;
using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class OrderRepository(DataContext dataContext, IMapper mapper) : IOrderRepository
{
    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken) => 
        await dataContext.Orders
            .AsNoTracking()
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .ThenInclude(line => line.Item)
            .OrderByDescending(order => order.OrderDate)
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

    public async Task<CustomOrderCommandResponse> GetCustomOrder(int orderNumber, CancellationToken cancellationToken)
    {
        return await dataContext.Orders
            .AsNoTracking()
            .Include(c => c.Customer)
            .ProjectTo<CustomOrderCommandResponse>(mapper.ConfigurationProvider)
            // .Select(o => new CustomOrderCommandResponse
            // {
            //     OrderNumber = o.OrderNumber,
            //     Status = o.Status,
            //     CustomerName = new CustorOrderCustomerResponse
            //     {
            //         CustomerName = o.Customer.FullName   
            //     }
            // })
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber) ?? throw new Exception("not found");
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
            PricingName = command.PricingName.Trim(),
            CustomerId = command.CustomerId,
            Items = await BuildLines(command.Items, cancellationToken),
            StockAdjusted = true
        };
        dataContext.Orders.Add(order);
        await ApplyFilamentDelta(EmptyFilament(), await Reservation(order.Status, order.Items, cancellationToken), cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await GetByOrderNumber(order.OrderNumber, cancellationToken);
    }
    public async Task<Order> UpdateOrder(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);
        
        var orderToUpdate = await dataContext.Orders
            .Include(order => order.Items)
            .ThenInclude(line => line.Item)
            .FirstOrDefaultAsync(o => o.OrderNumber == command.OrderNumber, cancellationToken);
        if (orderToUpdate is null) throw new Exception("order is not found");

        var before = orderToUpdate.StockAdjusted
            ? Reservation(orderToUpdate.Status, orderToUpdate.Items)
            : EmptyFilament();

        orderToUpdate.OrderDate = command.OrderDate;
        orderToUpdate.Status = command.Status ?? orderToUpdate.Status;
        orderToUpdate.PricingName = string.IsNullOrWhiteSpace(command.PricingName)
            ? orderToUpdate.PricingName
            : command.PricingName.Trim();
        orderToUpdate.CustomerId = command.CustomerId;
        dataContext.OrderItems.RemoveRange(orderToUpdate.Items);
        orderToUpdate.Items = await BuildLines(command.Items, cancellationToken);
        orderToUpdate.StockAdjusted = true;
        await ApplyFilamentDelta(before, await Reservation(orderToUpdate.Status, orderToUpdate.Items, cancellationToken), cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await GetByOrderNumber(orderToUpdate.OrderNumber, cancellationToken);
    }

    public async Task<bool> DeleteByOrderNumberAsync(int orderNumber, CancellationToken cancellationToken)
    {
        var order = await dataContext.Orders
            .Include(o => o.Items)
            .ThenInclude(line => line.Item)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
        if (order is null)
            return false;

        if (order.StockAdjusted)
            await ApplyFilamentDelta(Reservation(order.Status, order.Items), EmptyFilament(), cancellationToken);

        dataContext.Orders.Remove(order);
        await dataContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task ApplyOutstandingFilamentAsync(CancellationToken cancellationToken)
    {
        var orders = await dataContext.Orders
            .Include(order => order.Items)
            .ThenInclude(line => line.Item)
            .Where(order => !order.StockAdjusted)
            .ToListAsync(cancellationToken);

        foreach (var order in orders)
        {
            await ApplyFilamentDelta(EmptyFilament(), Reservation(order.Status, order.Items), cancellationToken);
            order.StockAdjusted = true;
        }

        await dataContext.SaveChangesAsync(cancellationToken);
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
            FilamentType = (line.FilamentType ?? "").Trim(),
            Color = (line.Color ?? "").Trim(),
            Quantity = line.Quantity,
            CompletedQuantity = Math.Clamp(line.CompletedQuantity, 0, line.Quantity)
        }).ToList();
    }

    private static Dictionary<(string Type, string Color), int> EmptyFilament() => new(FilamentKeyComparer.Instance);

    private static Dictionary<(string Type, string Color), int> Reservation(string status, IEnumerable<OrderItem> lines)
    {
        var totals = EmptyFilament();
        foreach (var line in lines)
        {
            var pieces = status == "Cancelled" ? line.CompletedQuantity : line.Quantity;
            var grams = (line.Item?.Weight ?? 0) * pieces;
            AddGrams(totals, line.FilamentType, line.Color, grams);
        }
        return totals;
    }

    private async Task<Dictionary<(string Type, string Color), int>> Reservation(
        string status,
        IEnumerable<OrderItem> lines,
        CancellationToken cancellationToken)
    {
        var list = lines.ToList();
        var ids = list.Select(line => line.ItemId).Distinct().ToList();
        var weights = await dataContext.Items
            .Where(item => ids.Contains(item.Id))
            .ToDictionaryAsync(item => item.Id, item => item.Weight, cancellationToken);

        var totals = EmptyFilament();
        foreach (var line in list)
        {
            var pieces = status == "Cancelled" ? line.CompletedQuantity : line.Quantity;
            weights.TryGetValue(line.ItemId, out var weight);
            AddGrams(totals, line.FilamentType, line.Color, weight * pieces);
        }
        return totals;
    }

    private static void AddGrams(Dictionary<(string Type, string Color), int> totals, string type, string color, int grams)
    {
        if (grams == 0)
            return;
        var key = (type.Trim(), color.Trim());
        totals[key] = totals.GetValueOrDefault(key) + grams;
    }

    private async Task ApplyFilamentDelta(
        Dictionary<(string Type, string Color), int> before,
        Dictionary<(string Type, string Color), int> after,
        CancellationToken cancellationToken)
    {
        var stock = await dataContext.FilamentStocks.ToListAsync(cancellationToken);
        foreach (var key in before.Keys.Union(after.Keys))
        {
            var consume = after.GetValueOrDefault(key) - before.GetValueOrDefault(key);
            if (consume == 0)
                continue;

            var matches = stock
                .Where(row => string.Equals(row.Type, key.Type, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(row.Color, key.Color, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(row => row.Quantity)
                .ToList();
            if (matches.Count == 0)
                continue;

            if (consume > 0)
            {
                var left = consume;
                foreach (var row in matches)
                {
                    var take = Math.Min(Math.Max(row.Quantity, 0), left);
                    row.Quantity -= take;
                    left -= take;
                    if (left == 0)
                        break;
                }
                if (left > 0)
                    matches[0].Quantity -= left;
            }
            else
            {
                matches[0].Quantity += -consume;
            }
        }
    }

    private sealed class FilamentKeyComparer : IEqualityComparer<(string Type, string Color)>
    {
        public static readonly FilamentKeyComparer Instance = new();

        public bool Equals((string Type, string Color) x, (string Type, string Color) y) =>
            string.Equals(x.Type, y.Type, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.Color, y.Color, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode((string Type, string Color) obj) =>
            HashCode.Combine(obj.Type.ToLowerInvariant(), obj.Color.ToLowerInvariant());
    }

}
