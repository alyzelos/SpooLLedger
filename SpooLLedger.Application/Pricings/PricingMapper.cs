using SpooLLedger.Application.Pricings.Commands;
using SpooLLedger.Application.Pricings.Dtos;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Pricings;

public static class PricingMapper
{
    public static PricingDto ToDto(Pricing pricing) => new()
    {
        Id = pricing.Id,
        Name = pricing.Name,
        ElectricityCostPerKwh = pricing.ElectricityCostPerKwh,
        FilamentSpoolPrice = pricing.FilamentSpoolPrice,
        FilamentCostPerGram = pricing.FilamentCostPerGram,
        PrinterMaintenanceCostPerHour = pricing.PrinterMaintenanceCostPerHour
    };

    public static Pricing ToEntity(CreatePricingCommand command) => new()
    {
        Name = command.Name,
        ElectricityCostPerKwh = command.ElectricityCostPerKwh,
        FilamentSpoolPrice = command.FilamentSpoolPrice,
        PrinterMaintenanceCostPerHour = command.PrinterMaintenanceCostPerHour
    };

    public static void Apply(UpdatePricingCommand command, Pricing pricing)
    {
        pricing.Name = command.Name;
        pricing.ElectricityCostPerKwh = command.ElectricityCostPerKwh;
        pricing.FilamentSpoolPrice = command.FilamentSpoolPrice;
        pricing.PrinterMaintenanceCostPerHour = command.PrinterMaintenanceCostPerHour;
    }
}
