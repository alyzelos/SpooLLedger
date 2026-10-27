using MediatR;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public enum DeleteFilamentTypeResult
{
    Deleted,
    NotFound,
    UsedByStock
}

public record DeleteFilamentTypeCommand(int Id) : IRequest<DeleteFilamentTypeResult>;
