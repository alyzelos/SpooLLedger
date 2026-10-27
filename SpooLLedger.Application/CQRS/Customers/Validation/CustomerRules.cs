using FluentValidation;

namespace SpooLLedger.Application.CQRS.Customers.Validation;

public static class CustomerRules
{
    public static void Apply<T>(
        IRuleBuilderInitial<T, string> firstName,
        IRuleBuilderInitial<T, string> lastName,
        IRuleBuilderInitial<T, string> email,
        IRuleBuilderInitial<T, string> phoneNumber)
    {
        firstName.NotEmpty().MaximumLength(100);
        lastName.NotEmpty().MaximumLength(100);
        email.NotEmpty().MaximumLength(200).EmailAddress();
        phoneNumber.NotEmpty().MaximumLength(30);
    }
}
