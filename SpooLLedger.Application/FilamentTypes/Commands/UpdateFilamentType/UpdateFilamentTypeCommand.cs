using MediatR;
using SpooLLedger.Application.FilamentTypes.Dtos;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public record UpdateFilamentTypeCommand(int Id, string Name) : IRequest<FilamentTypeDto?>;
