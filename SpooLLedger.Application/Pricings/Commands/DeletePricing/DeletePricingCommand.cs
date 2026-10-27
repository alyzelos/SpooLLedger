using MediatR;

namespace SpooLLedger.Application.Pricings.Commands;

public record DeletePricingCommand(int Id) : IRequest<bool>;
