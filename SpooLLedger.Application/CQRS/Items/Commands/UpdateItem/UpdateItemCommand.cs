using MediatR;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public record UpdateItemCommand(int Id, string ProductName, int Weight, int PrintDurationMinutes, string? Details)
    : IRequest<ItemDto?>;
