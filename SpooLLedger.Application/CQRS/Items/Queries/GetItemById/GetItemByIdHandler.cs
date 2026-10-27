using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Queries.GetItemById;

public class GetItemByIdHandler(IItemsRepository repository) : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        return item is null ? null : ItemMapper.ToDto(item);
    }
}
