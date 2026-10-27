using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Pricings;

public interface IItemPriceCalculator
{
    ItemPriceBreakdown Calculate(Item item, Pricing pricing);
}

public record ItemPriceBreakdown(
    decimal Filament,
    decimal Electricity,
    decimal Maintenance,
    decimal Total,
    decimal EnergyKwh);
