using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentColors.Commands;

public class UpdateFilamentColorValidator : AbstractValidator<UpdateFilamentColorCommand>
{
    public UpdateFilamentColorValidator(IFilamentColorRepository repository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await repository.NameTakenAsync(name.Trim(), command.Id, cancellationToken))
            .WithMessage("A filament color named '{PropertyValue}' already exists.");
    }
}
