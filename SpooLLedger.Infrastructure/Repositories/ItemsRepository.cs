using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Items.Commands.Create;
using SpooLLedger.Application.CQRS.Items.Queries;
using SpooLLedger.Application.CQRS.Items.Responses;
using SpooLLedger.Domain.Entities;
using SpooLLedger.Infrastructure.Data.DatabaseContext;

namespace SpooLLedger.Infrastructure.Repositories;

public class ItemsRepository(DataContext dataContext) : IItemsRepository
{

    public async Task<IReadOnlyList<Item>> GetAllItems(CancellationToken cancellationToken)
    {
        return await dataContext.Items
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Item> GetItemById(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await dataContext.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken) ?? throw new Exception("item was not found in db");
        return item;
    }

    public async Task<Item> CreateItem(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            ProductName = command.ProductName,
            Weight = command.Weight
        };
        dataContext.Items.Add(item);
        await dataContext.SaveChangesAsync(cancellationToken);
        return  item;
    }
}
