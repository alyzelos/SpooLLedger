using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentTypes.Commands;

public class DeleteFilamentTypeHandler(IFilamentTypeRepository repository)
    : IRequestHandler<DeleteFilamentTypeCommand, DeleteFilamentTypeResult>
{
    public async Task<DeleteFilamentTypeResult> Handle(DeleteFilamentTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (type is null)
            return DeleteFilamentTypeResult.NotFound;

        if (await repository.IsUsedByStockAsync(type.Name, cancellationToken))
            return DeleteFilamentTypeResult.UsedByStock;

        await repository.DeleteAsync(type, cancellationToken);
        return DeleteFilamentTypeResult.Deleted;
    }
}
