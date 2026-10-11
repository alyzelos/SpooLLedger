using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Commands;

public class UpdateFilamentStockHandler(IFilamentStockRepository repository)
    : IRequestHandler<UpdateFilamentStockCommand, FilamentStockDto?>
{
    public async Task<FilamentStockDto?> Handle(UpdateFilamentStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (stock is null)
            return null;

        FilamentStockMapper.Apply(request, stock);
        await repository.UpdateAsync(stock, cancellationToken);
        return FilamentStockMapper.ToDto(stock);
    }
}
