using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentTypes.Dtos;

namespace SpooLLedger.Application.FilamentTypes.Queries.GetFilamentTypes;

public class GetFilamentTypesHandler(IFilamentTypeRepository repository)
    : IRequestHandler<GetFilamentTypesQuery, IReadOnlyList<FilamentTypeDto>>
{
    public async Task<IReadOnlyList<FilamentTypeDto>> Handle(GetFilamentTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await repository.GetAllAsync(cancellationToken);
        return types.Select(t => new FilamentTypeDto(t.Id, t.Name)).ToList();
    }
}
