using MediatR;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public record CalculateItemPriceCommand(int Id, string PricingName) : IRequest<ItemPriceBreakdown?>;
