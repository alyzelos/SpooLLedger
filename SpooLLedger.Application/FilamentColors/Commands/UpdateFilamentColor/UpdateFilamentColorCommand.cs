using MediatR;
using SpooLLedger.Application.FilamentColors.Dtos;

namespace SpooLLedger.Application.FilamentColors.Commands;

public record UpdateFilamentColorCommand(int Id, string Name) : IRequest<FilamentColorDto?>;
