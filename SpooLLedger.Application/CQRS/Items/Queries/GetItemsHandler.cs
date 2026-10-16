using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Responses;

namespace SpooLLedger.Application.CQRS.Items.Queries;

public class GetItemsHandler(IItemsRepository itemsRepository, IMapper mapper) : IRequestHandler<GetItemsQuery, IReadOnlyList<GetItemResponse>>
{
    public async Task<IReadOnlyList<GetItemResponse>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
    {
        var result = await itemsRepository.GetAllItems(cancellationToken);
        return mapper.Map<IReadOnlyList<GetItemResponse>>(result); 
    }
}
