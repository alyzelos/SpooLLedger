using FluentValidation;

namespace SpooLLedger.Application.CQRS.Items.Validation;

public static class ItemRules
{
    public const int MaxNameLength = 100;
    public const int MaxDetailsLength = 1000;

    public static void ApplyItemRules<T>(
        IRuleBuilderInitial<T, string> productName,
        IRuleBuilderInitial<T, int> weight,
        IRuleBuilderInitial<T, int> printDurationMinutes,
        IRuleBuilderInitial<T, string?> details)
    {
        productName.NotEmpty().MaximumLength(MaxNameLength);
        weight.GreaterThan(0);
        printDurationMinutes.GreaterThan(0);
        details.MaximumLength(MaxDetailsLength);
    }
}
