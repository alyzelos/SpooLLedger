using System;
using SpooLLedger.Application.CQRS.Items.Commands.Create;
using SpooLLedger.Application.CQRS.Items.Queries;
using SpooLLedger.Application.CQRS.Items.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.Abstractions;

public interface IItemsRepository
{
    public Task<IReadOnlyList<Item>> GetAllItems(CancellationToken cancellationToken);
    public Task<Item> GetItemById(GetItemByIdQuery query, CancellationToken cancellationToken);
    public Task<Item> CreateItem(CreateItemCommand command, CancellationToken cancellationToken);
}
