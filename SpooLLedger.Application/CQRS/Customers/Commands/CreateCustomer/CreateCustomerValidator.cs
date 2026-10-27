using FluentValidation;
using SpooLLedger.Application.CQRS.Customers.Validation;

namespace SpooLLedger.Application.CQRS.Customers.Commands;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        CustomerRules.Apply(
            RuleFor(x => x.FirstName),
            RuleFor(x => x.LastName),
            RuleFor(x => x.Email),
            RuleFor(x => x.PhoneNumber));
    }
}
