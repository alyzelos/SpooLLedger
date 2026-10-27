using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Commands;

public class CreateItemHandler(IItemsRepository repository) : IRequestHandler<CreateItemCommand, ItemDto>
{
    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = ItemMapper.ToEntity(request);
        await repository.AddAsync(item, cancellationToken);
        return ItemMapper.ToDto(item);
    }
}
