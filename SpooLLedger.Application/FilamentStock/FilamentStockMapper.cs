using SpooLLedger.Application.FilamentStock.Commands;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock;

public static class FilamentStockMapper
{
    public static FilamentStockDto ToDto(Domain.FilamentStock stock) => new()
    {
        Id = stock.Id,
        Producer = stock.Producer,
        Type = stock.Type,
        Color = stock.Color,
        Quantity = stock.Quantity
    };

    public static Domain.FilamentStock ToEntity(CreateFilamentStockCommand command) => new()
    {
        Producer = command.Producer,
        Type = command.Type,
        Color = command.Color,
        Quantity = command.Quantity
    };

    public static void Apply(UpdateFilamentStockCommand command, Domain.FilamentStock stock)
    {
        stock.Producer = command.Producer;
        stock.Type = command.Type;
        stock.Color = command.Color;
        stock.Quantity = command.Quantity;
    }
}
