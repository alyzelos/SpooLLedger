using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.CQRS.Order;

public static class OrderPrices
{
    public static async Task Apply(
        IEnumerable<OrderResponseBase> orders,
        IPricingRepository pricings,
        IItemPriceCalculator calculator,
        CancellationToken cancellationToken)
    {
        var list = orders.ToList();
        var catalog = await pricings.GetAllAsync(cancellationToken);
        var byName = catalog.ToDictionary(pricing => pricing.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var order in list)
        {
            if (!byName.TryGetValue((order.PricingName ?? "").Trim(), out var pricing))
                continue;

            foreach (var line in order.Items)
            {
                var unit = decimal.Round(
                    calculator.Calculate(
                        new Item { Weight = line.Weight, PrintDurationMinutes = line.PrintDurationMinutes },
                        pricing).Total,
                    2,
                    MidpointRounding.AwayFromZero);
                line.PricePerUnit = unit;
                line.TotalPrice = unit * line.Quantity;
            }
        }
    }
}
