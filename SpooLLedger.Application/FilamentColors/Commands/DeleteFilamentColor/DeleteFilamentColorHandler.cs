using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentColors.Commands;

public class DeleteFilamentColorHandler(IFilamentColorRepository repository)
    : IRequestHandler<DeleteFilamentColorCommand, DeleteFilamentColorResult>
{
    public async Task<DeleteFilamentColorResult> Handle(DeleteFilamentColorCommand request, CancellationToken cancellationToken)
    {
        var color = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (color is null)
            return DeleteFilamentColorResult.NotFound;

        if (await repository.IsUsedByStockAsync(color.Name, cancellationToken))
            return DeleteFilamentColorResult.UsedByStock;

        await repository.DeleteAsync(color, cancellationToken);
        return DeleteFilamentColorResult.Deleted;
    }
}
