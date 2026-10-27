using FluentValidation;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public class UpdateFilamentTypeValidator : AbstractValidator<UpdateFilamentTypeCommand>
{
    public UpdateFilamentTypeValidator(IFilamentTypeRepository repository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (command, name, cancellationToken) =>
                !await repository.NameTakenAsync(name.Trim(), command.Id, cancellationToken))
            .WithMessage("A filament type named '{PropertyValue}' already exists.");
    }
}
