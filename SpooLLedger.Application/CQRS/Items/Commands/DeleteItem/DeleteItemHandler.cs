using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class DeleteItemHandler(IItemsRepository repository) : IRequestHandler<DeleteItemCommand, DeleteItemResult>
{
    public async Task<DeleteItemResult> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return DeleteItemResult.NotFound;

        if (await repository.IsUsedInOrdersAsync(request.Id, cancellationToken))
            return DeleteItemResult.UsedInOrders;

        await repository.DeleteAsync(item, cancellationToken);
        return DeleteItemResult.Deleted;
    }
}
