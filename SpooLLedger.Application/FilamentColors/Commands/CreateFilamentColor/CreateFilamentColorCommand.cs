using MediatR;
using SpooLLedger.Application.FilamentColors.Dtos;

namespace SpooLLedger.Application.FilamentColors.Commands;

public record CreateFilamentColorCommand(string Name) : IRequest<FilamentColorDto>;
