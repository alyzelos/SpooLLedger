using FluentValidation;
using SpooLLedger.Application.CQRS.Items.Validation;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class UpdateItemValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        ItemRules.ApplyItemRules(
            RuleFor(x => x.ProductName),
            RuleFor(x => x.Weight),
            RuleFor(x => x.PrintDurationMinutes),
            RuleFor(x => x.Details));
    }
}
