using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Queries.GetFilamentStocks;

public class GetFilamentStocksHandler(IFilamentStockRepository repository)
    : IRequestHandler<GetFilamentStocksQuery, IReadOnlyList<FilamentStockDto>>
{
    public async Task<IReadOnlyList<FilamentStockDto>> Handle(GetFilamentStocksQuery request, CancellationToken cancellationToken)
    {
        var stocks = await repository.GetAllAsync(cancellationToken);
        return stocks.Select(FilamentStockMapper.ToDto).ToList();
    }
}
