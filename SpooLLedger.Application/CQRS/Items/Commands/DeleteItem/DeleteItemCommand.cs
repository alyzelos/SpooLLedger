using MediatR;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public enum DeleteItemResult
{
    Deleted,
    NotFound,
    UsedInOrders
}

public record DeleteItemCommand(int Id) : IRequest<DeleteItemResult>;
