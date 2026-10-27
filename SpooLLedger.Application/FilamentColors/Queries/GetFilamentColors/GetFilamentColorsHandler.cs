using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentColors.Dtos;

namespace SpooLLedger.Application.FilamentColors.Queries.GetFilamentColors;

public class GetFilamentColorsHandler(IFilamentColorRepository repository)
    : IRequestHandler<GetFilamentColorsQuery, IReadOnlyList<FilamentColorDto>>
{
    public async Task<IReadOnlyList<FilamentColorDto>> Handle(GetFilamentColorsQuery request, CancellationToken cancellationToken)
    {
        var colors = await repository.GetAllAsync(cancellationToken);
        return colors.Select(color => new FilamentColorDto(color.Id, color.Name)).ToList();
    }
}
