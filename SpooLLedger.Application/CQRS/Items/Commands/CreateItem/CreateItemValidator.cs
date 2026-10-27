using FluentValidation;
using SpooLLedger.Application.CQRS.Items.Validation;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        ItemRules.ApplyItemRules(
            RuleFor(x => x.ProductName),
            RuleFor(x => x.Weight),
            RuleFor(x => x.PrintDurationMinutes),
            RuleFor(x => x.Details));
    }
}
