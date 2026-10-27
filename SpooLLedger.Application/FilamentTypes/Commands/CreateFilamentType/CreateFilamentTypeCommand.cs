using MediatR;
using SpooLLedger.Application.FilamentTypes.Dtos;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public record CreateFilamentTypeCommand(string Name) : IRequest<FilamentTypeDto>;
