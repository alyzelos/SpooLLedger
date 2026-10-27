using MediatR;
using SpooLLedger.Application.FilamentColors.Dtos;

namespace SpooLLedger.Application.FilamentColors.Queries.GetFilamentColors;

public record GetFilamentColorsQuery : IRequest<IReadOnlyList<FilamentColorDto>>;
