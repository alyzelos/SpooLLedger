using FluentValidation;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Validation;

namespace SpooLLedger.Application.Pricings.Commands;

public class CreatePricingValidator : AbstractValidator<CreatePricingCommand>
{
    public CreatePricingValidator(IPricingRepository repository)
    {
        PricingRules.ApplyPricingRules(
            RuleFor(x => x.ElectricityCostPerKwh),
            RuleFor(x => x.FilamentSpoolPrice),
            RuleFor(x => x.PrinterMaintenanceCostPerHour));

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(PricingRules.MaxNameLength)
            .MustAsync(async (name, cancellationToken) =>
                !await repository.NameExistsAsync(name, null, cancellationToken))
            .WithMessage("A pricing named '{PropertyValue}' already exists.");
    }
}
