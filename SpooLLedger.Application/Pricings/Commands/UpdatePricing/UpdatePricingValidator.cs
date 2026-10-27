using FluentValidation;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Validation;

namespace SpooLLedger.Application.Pricings.Commands;

public class UpdatePricingValidator : AbstractValidator<UpdatePricingCommand>
{
    public UpdatePricingValidator(IPricingRepository repository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        PricingRules.ApplyPricingRules(
            RuleFor(x => x.ElectricityCostPerKwh),
            RuleFor(x => x.FilamentSpoolPrice),
            RuleFor(x => x.PrinterMaintenanceCostPerHour));

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(PricingRules.MaxNameLength)
            .MustAsync(async (command, name, cancellationToken) =>
                !await repository.NameExistsAsync(name, command.Id, cancellationToken))
            .WithMessage("A pricing named '{PropertyValue}' already exists.");
    }
}
