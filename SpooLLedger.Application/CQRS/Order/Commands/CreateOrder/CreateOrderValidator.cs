using System;
using FluentValidation;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(c => c.OrderDate).NotEmpty();
        RuleFor(c => c.Status).Equal("Pending");
        RuleFor(c => c.CustomerId).NotEmpty();
        // RuleFor(c => c.Customer).NotEmpty();
    }

}
