using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentTypes.Dtos;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public class CreateFilamentTypeHandler(IFilamentTypeRepository repository)
    : IRequestHandler<CreateFilamentTypeCommand, FilamentTypeDto>
{
    public async Task<FilamentTypeDto> Handle(CreateFilamentTypeCommand request, CancellationToken cancellationToken)
    {
        var type = new FilamentType { Name = request.Name.Trim() };
        await repository.AddAsync(type, cancellationToken);
        return new FilamentTypeDto(type.Id, type.Name);
    }
}
