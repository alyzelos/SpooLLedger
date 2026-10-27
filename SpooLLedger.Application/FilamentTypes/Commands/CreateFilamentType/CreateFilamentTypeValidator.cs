using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public class CreateFilamentTypeValidator : AbstractValidator<CreateFilamentTypeCommand>
{
    public CreateFilamentTypeValidator(IFilamentTypeRepository repository)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (name, cancellationToken) =>
                !await repository.NameTakenAsync(name.Trim(), null, cancellationToken))
            .WithMessage("A filament type named '{PropertyValue}' already exists.");
    }
}
