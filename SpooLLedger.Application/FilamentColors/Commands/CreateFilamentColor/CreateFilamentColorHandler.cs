using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentColors.Dtos;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.FilamentColors.Commands;

public class CreateFilamentColorHandler(IFilamentColorRepository repository)
    : IRequestHandler<CreateFilamentColorCommand, FilamentColorDto>
{
    public async Task<FilamentColorDto> Handle(CreateFilamentColorCommand request, CancellationToken cancellationToken)
    {
        var color = new FilamentColor { Name = request.Name.Trim() };
        await repository.AddAsync(color, cancellationToken);
        return new FilamentColorDto(color.Id, color.Name);
    }
}
