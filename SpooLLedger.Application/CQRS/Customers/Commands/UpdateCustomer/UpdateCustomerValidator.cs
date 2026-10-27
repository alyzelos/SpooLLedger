using FluentValidation;
using SpooLLedger.Application.CQRS.Customers.Validation;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        CustomerRules.Apply(
            RuleFor(x => x.FirstName),
            RuleFor(x => x.LastName),
            RuleFor(x => x.Email),
            RuleFor(x => x.PhoneNumber));
    }
}
