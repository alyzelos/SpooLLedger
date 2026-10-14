using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IOrderRspository
{
    public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Order> GetByOrderNumber(int orderNumber, CancellationToken cancellationToken);
    public Task<Order> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken);
}
