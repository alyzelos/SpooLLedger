using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Queries;
using SpooLLedger.Application.CQRS.Items.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.CQRS.Items.Commands.Create;

public class CreateItemHandler(IItemsRepository itemsRepository, IMapper mapper) : IRequestHandler<CreateItemCommand, CreateItemResponse>
{
    public async Task<CreateItemResponse> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {   
        var createdItem = await itemsRepository.CreateItem(command, cancellationToken);
        return mapper.Map<CreateItemResponse>(createdItem);
    }
}
