using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentStock.Validation;

public static class FilamentStockRules
{
    public const int MaxTextLength = 100;

    public static void ApplyStockRules<T>(
        IRuleBuilderInitial<T, string> producer,
        IRuleBuilderInitial<T, string> type,
        IRuleBuilderInitial<T, string> color,
        IRuleBuilderInitial<T, int> quantity,
        IFilamentTypeRepository filamentTypes,
        IFilamentColorRepository filamentColors)
    {
        producer.NotEmpty().MaximumLength(MaxTextLength);
        type.Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(MaxTextLength)
            .MustAsync(filamentTypes.ExistsAsync)
            .WithMessage("'{PropertyValue}' is not a known filament type.");
        color.Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(MaxTextLength)
            .MustAsync(filamentColors.ExistsAsync)
            .WithMessage("'{PropertyValue}' is not a known filament color.");
        quantity.GreaterThanOrEqualTo(0);
    }
}
