using FluentValidation;
using SpooLLedger.Application.FilamentStock.Validation;

namespace SpooLLedger.Application.FilamentStock.Commands;

public class CreateFilamentStockValidator : AbstractValidator<CreateFilamentStockCommand>
{
    public CreateFilamentStockValidator()
    {
        FilamentStockRules.ApplyStockRules(RuleFor(x => x.Producer), RuleFor(x => x.Type), RuleFor(x => x.Color), RuleFor(x => x.Quantity));
    }
}
