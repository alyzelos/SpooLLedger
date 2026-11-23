using MediatR;

namespace SpooLLedger.Application.FilamentColors.Commands;

public enum DeleteFilamentColorResult
{
    Deleted,
    NotFound,
    UsedByStock
}

public record DeleteFilamentColorCommand(int Id) : IRequest<DeleteFilamentColorResult>;
