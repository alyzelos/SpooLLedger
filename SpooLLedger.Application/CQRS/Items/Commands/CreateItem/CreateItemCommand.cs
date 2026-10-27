using MediatR;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public record CreateItemCommand(string ProductName, int Weight, int PrintDurationMinutes, string? Details)
    : IRequest<ItemDto>;
