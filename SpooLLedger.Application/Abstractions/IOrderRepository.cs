using SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;
using SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IOrderRepository
{
    public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Order> GetByOrderNumber(int orderNumber, CancellationToken cancellationToken);
    public Task<Order> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken);
    public Task<Order> UpdateOrder(UpdateOrderCommand command, CancellationToken cancellationToken);

}
