using MediatR;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Queries.GetItems;

public record GetItemsQuery : IRequest<IReadOnlyList<ItemDto>>;
