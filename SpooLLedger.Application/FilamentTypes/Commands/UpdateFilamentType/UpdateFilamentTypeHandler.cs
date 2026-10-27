using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentTypes.Dtos;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public class UpdateFilamentTypeHandler(IFilamentTypeRepository repository)
    : IRequestHandler<UpdateFilamentTypeCommand, FilamentTypeDto?>
{
    public async Task<FilamentTypeDto?> Handle(UpdateFilamentTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (type is null)
            return null;

        var previousName = type.Name;
        type.Name = request.Name.Trim();
        await repository.UpdateAsync(type, previousName, cancellationToken);
        return new FilamentTypeDto(type.Id, type.Name);
    }
}
