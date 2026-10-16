using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Responses;

namespace SpooLLedger.Application.CQRS.Items.Queries;

public class GetItemByIdHandler(IItemsRepository itemsRepository, IMapper mapper) : IRequestHandler<GetItemByIdQuery, GetItemResponse>
{
    public async Task<GetItemResponse> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await itemsRepository.GetItemById(query, cancellationToken);
        return mapper.Map<GetItemResponse>(item);
    }
}
