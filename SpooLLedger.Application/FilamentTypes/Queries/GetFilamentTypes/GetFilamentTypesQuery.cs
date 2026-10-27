using MediatR;
using SpooLLedger.Application.FilamentTypes.Dtos;

namespace SpooLLedger.Application.FilamentTypes.Queries.GetFilamentTypes;

public record GetFilamentTypesQuery : IRequest<IReadOnlyList<FilamentTypeDto>>;
