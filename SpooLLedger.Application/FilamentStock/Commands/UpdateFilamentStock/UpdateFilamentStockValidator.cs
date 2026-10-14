using FluentValidation;
using SpooLLedger.Application.FilamentStock.Validation;

namespace SpooLLedger.Application.FilamentStock.Commands;

public class UpdateFilamentStockValidator : AbstractValidator<UpdateFilamentStockCommand>
{
    public UpdateFilamentStockValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        FilamentStockRules.ApplyStockRules(
            RuleFor(x => x.Producer), 
            RuleFor(x => x.Type), 
            RuleFor(x => x.Color), 
            RuleFor(x => x.Quantity));
    }
}
