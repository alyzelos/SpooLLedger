using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentColors.Dtos;

namespace SpooLLedger.Application.FilamentColors.Commands;

public class UpdateFilamentColorHandler(IFilamentColorRepository repository)
    : IRequestHandler<UpdateFilamentColorCommand, FilamentColorDto?>
{
    public async Task<FilamentColorDto?> Handle(UpdateFilamentColorCommand request, CancellationToken cancellationToken)
    {
        var color = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (color is null)
            return null;

        var previousName = color.Name;
        color.Name = request.Name.Trim();
        await repository.UpdateAsync(color, previousName, cancellationToken);
        return new FilamentColorDto(color.Id, color.Name);
    }
}
