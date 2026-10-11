using MediatR;

namespace SpooLLedger.Application.FilamentStock.Commands;

public record DeleteFilamentStockCommand(int Id) : IRequest<bool>;
