using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class UpdateItemHandler(IItemsRepository repository) : IRequestHandler<UpdateItemCommand, ItemDto?>
{
    public async Task<ItemDto?> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return null;

        ItemMapper.Apply(request, item);
        await repository.UpdateAsync(item, cancellationToken);
        return ItemMapper.ToDto(item);
    }
}
