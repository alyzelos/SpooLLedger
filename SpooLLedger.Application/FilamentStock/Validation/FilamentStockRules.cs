using FluentValidation;

namespace SpooLLedger.Application.FilamentStock.Validation;

public static class FilamentStockRules
{
    public const int MaxTextLength = 100;

    public static void ApplyStockRules<T>(
        IRuleBuilderInitial<T, string> producer,
        IRuleBuilderInitial<T, string> type,
        IRuleBuilderInitial<T, string> color,
        IRuleBuilderInitial<T, int> quantity)
    {
        producer.NotEmpty().MaximumLength(MaxTextLength);
        type.NotEmpty().MaximumLength(MaxTextLength);
        color.NotEmpty().MaximumLength(MaxTextLength);
        quantity.GreaterThanOrEqualTo(0);
    }
}
