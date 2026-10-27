using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Pricings;

public class ItemPriceCalculator : IItemPriceCalculator
{
    public const int HeatUpMinutes = 10;
    public const decimal HeatUpWatts = 120m;
    public const decimal PrintingWatts = 80m;

    public ItemPriceBreakdown Calculate(Item item, Pricing pricing)
    {
        var filament = item.Weight * pricing.FilamentCostPerGram;

        var hours = item.PrintDurationMinutes / 60m;
        var maintenance = hours * pricing.PrinterMaintenanceCostPerHour;

        var heatUp = Math.Min(item.PrintDurationMinutes, HeatUpMinutes);
        var printing = Math.Max(item.PrintDurationMinutes - HeatUpMinutes, 0);
        var energyKwh = WattsToKwh(heatUp, HeatUpWatts) + WattsToKwh(printing, PrintingWatts);
        var electricity = energyKwh * pricing.ElectricityCostPerKwh;

        return new ItemPriceBreakdown(
            filament,
            electricity,
            maintenance,
            filament + electricity + maintenance,
            energyKwh);
    }

    private static decimal WattsToKwh(int minutes, decimal watts) =>
        minutes / 60m * watts / 1000m;
}
