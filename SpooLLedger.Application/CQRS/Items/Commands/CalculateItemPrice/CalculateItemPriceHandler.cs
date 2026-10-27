using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class CalculateItemPriceHandler(IItemsRepository items, IPricingRepository pricings, IItemPriceCalculator calculator)
    : IRequestHandler<CalculateItemPriceCommand, ItemPriceBreakdown?>
{
    public async Task<ItemPriceBreakdown?> Handle(CalculateItemPriceCommand request, CancellationToken cancellationToken)
    {
        var item = await items.GetByIdAsync(request.Id, cancellationToken);
        var pricing = await pricings.GetByNameAsync(request.PricingName, cancellationToken);
        if (item is null || pricing is null)
            return null;

        var breakdown = calculator.Calculate(item, pricing);
        item.PricePerUnit = decimal.Round(breakdown.Total, 2, MidpointRounding.AwayFromZero);
        await items.UpdateAsync(item, cancellationToken);
        return breakdown;
    }
}
