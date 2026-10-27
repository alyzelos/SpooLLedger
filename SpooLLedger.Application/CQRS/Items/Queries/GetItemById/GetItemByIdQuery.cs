using MediatR;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Queries.GetItemById;

public record GetItemByIdQuery(int Id) : IRequest<ItemDto?>;
