using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class CalculateItemPriceValidator : AbstractValidator<CalculateItemPriceCommand>
{
    public CalculateItemPriceValidator(IPricingRepository pricings)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.PricingName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (name, cancellationToken) =>
                await pricings.GetByNameAsync(name, cancellationToken) is not null)
            .WithMessage("'{PropertyValue}' is not a known pricing.");
    }
}
