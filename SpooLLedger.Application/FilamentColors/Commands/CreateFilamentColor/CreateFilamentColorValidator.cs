using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentColors.Commands;

public class CreateFilamentColorValidator : AbstractValidator<CreateFilamentColorCommand>
{
    public CreateFilamentColorValidator(IFilamentColorRepository repository)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (name, cancellationToken) =>
                !await repository.NameTakenAsync(name.Trim(), null, cancellationToken))
            .WithMessage("A filament color named '{PropertyValue}' already exists.");
    }
}
