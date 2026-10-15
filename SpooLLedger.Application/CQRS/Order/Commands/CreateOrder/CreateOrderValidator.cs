using System;
using FluentValidation;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(c => c.OrderDate).NotEmpty();
        RuleFor(c => c.Status).Equal("Pending");
        RuleFor(c => c.CustomerId).GreaterThan(0);
        RuleForEach(c => c.Items).ChildRules(line =>
        {
            line.RuleFor(item => item.ItemId).GreaterThan(0);
            line.RuleFor(item => item.Quantity).GreaterThan(0);
        });
    }

}
