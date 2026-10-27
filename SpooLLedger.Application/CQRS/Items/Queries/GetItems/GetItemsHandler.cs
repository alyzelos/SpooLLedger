using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Dtos;

namespace SpooLLedger.Application.CQRS.Items.Queries.GetItems;

public class GetItemsHandler(IItemsRepository repository) : IRequestHandler<GetItemsQuery, IReadOnlyList<ItemDto>>
{
    public async Task<IReadOnlyList<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAsync(cancellationToken);
        return items.Select(ItemMapper.ToDto).ToList();
    }
}
