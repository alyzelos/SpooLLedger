using FluentValidation;

namespace SpooLLedger.Application.Pricings.Validation;

public static class PricingRules
{
    public const int MaxNameLength = 100;

    public static void ApplyPricingRules<T>(
        IRuleBuilderInitial<T, decimal> electricityCostPerKwh,
        IRuleBuilderInitial<T, decimal> filamentSpoolPrice,
        IRuleBuilderInitial<T, decimal> printerMaintenanceCostPerHour)
    {
        electricityCostPerKwh.GreaterThanOrEqualTo(0);
        filamentSpoolPrice.GreaterThanOrEqualTo(0);
        printerMaintenanceCostPerHour.GreaterThanOrEqualTo(0);
    }
}
