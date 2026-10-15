using FluentValidation;

namespace SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(c => c.OrderNumber).GreaterThan(0);
        RuleFor(c => c.OrderDate).NotEmpty();
        RuleFor(c => c.Status).NotEmpty().MaximumLength(30);
        RuleFor(c => c.CustomerId).GreaterThan(0);
        RuleForEach(c => c.Items).ChildRules(line =>
        {
            line.RuleFor(item => item.ItemId).GreaterThan(0);
            line.RuleFor(item => item.Quantity).GreaterThan(0);
        });
    }
}
