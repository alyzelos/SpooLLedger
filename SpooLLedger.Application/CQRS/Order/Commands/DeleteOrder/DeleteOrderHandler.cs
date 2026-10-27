using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.CQRS.Order.Commands.DeleteOrder;

public class DeleteOrderHandler(IOrderRepository repository) : IRequestHandler<DeleteOrderCommand, bool>
{
    public Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken) =>
        repository.DeleteByOrderNumberAsync(request.OrderNumber, cancellationToken);
}
