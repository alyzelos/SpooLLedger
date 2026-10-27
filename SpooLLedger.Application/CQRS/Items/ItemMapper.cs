using SpooLLedger.Application.CQRS.Items.Commands;
using SpooLLedger.Application.CQRS.Items.Dtos;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.CQRS.Items;

public static class ItemMapper
{
    public static ItemDto ToDto(Item item) => new()
    {
        Id = item.Id,
        ProductName = item.ProductName,
        Weight = item.Weight,
        PrintDurationMinutes = item.PrintDurationMinutes,
        Details = item.Details,
        PricePerUnit = item.PricePerUnit
    };

    public static Item ToEntity(CreateItemCommand command) => new()
    {
        ProductName = command.ProductName,
        Weight = command.Weight,
        PrintDurationMinutes = command.PrintDurationMinutes,
        Details = command.Details
    };

    public static void Apply(UpdateItemCommand command, Item item)
    {
        item.ProductName = command.ProductName;
        item.Weight = command.Weight;
        item.PrintDurationMinutes = command.PrintDurationMinutes;
        item.Details = command.Details;
    }
}
