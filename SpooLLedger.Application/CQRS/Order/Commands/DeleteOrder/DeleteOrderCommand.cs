using MediatR;

namespace SpooLLedger.Application.CQRS.Order.Commands.DeleteOrder;

public record DeleteOrderCommand(int OrderNumber) : IRequest<bool>;
