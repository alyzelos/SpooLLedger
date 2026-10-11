using MediatR;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Commands;

public record UpdateFilamentStockCommand(int Id, string Producer, string Type, string Color, int Quantity)
    : IRequest<FilamentStockDto?>;
