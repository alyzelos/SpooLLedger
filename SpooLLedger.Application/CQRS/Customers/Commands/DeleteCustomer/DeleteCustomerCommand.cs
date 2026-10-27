using MediatR;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public enum DeleteCustomerResult
{
    Deleted,
    NotFound,
    UsedInOrders
}

public record DeleteCustomerCommand(int Id) : IRequest<DeleteCustomerResult>;
