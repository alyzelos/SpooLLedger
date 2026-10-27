using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;

public class CreateOrderSoapValidator : AbstractValidator<CreateOrderSoapCommand>
{
    public CreateOrderSoapValidator(
        IFilamentTypeRepository filamentTypes,
        IFilamentColorRepository filamentColors,
        IPricingRepository pricings)
    {
        RuleFor(c => c.OrderDate).NotEmpty();
        RuleFor(c => c.Status).Equal("Pending");
        RuleFor(c => c.PricingName).NotEmpty().MaximumLength(100);
        RuleFor(c => c.PricingName)
            .MustAsync((name, cancellationToken) => pricings.NameExistsAsync((name ?? "").Trim(), null, cancellationToken))
            .WithMessage("Choose a pricing that exists.");
        RuleFor(c => c.CustomerId).GreaterThan(0);
        RuleForEach(c => c.Items).ChildRules(line =>
        {
            line.RuleFor(item => item.ItemId).GreaterThan(0);
            line.RuleFor(item => item.FilamentType).NotEmpty().MaximumLength(100);
            line.RuleFor(item => item.FilamentType)
                .MustAsync((type, cancellationToken) => filamentTypes.ExistsAsync((type ?? "").Trim(), cancellationToken))
                .WithMessage("Choose a filament type that exists.");
            line.RuleFor(item => item.Color).NotEmpty().MaximumLength(100);
            line.RuleFor(item => item.Color)
                .MustAsync((color, cancellationToken) => filamentColors.ExistsAsync((color ?? "").Trim(), cancellationToken))
                .WithMessage("Choose a filament color that exists.");
            line.RuleFor(item => item.Quantity).GreaterThan(0);
        });
    }
}
