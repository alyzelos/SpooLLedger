using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Queries.GetFilamentStockById;

public class GetFilamentStockByIdHandler(IFilamentStockRepository repository)
    : IRequestHandler<GetFilamentStockByIdQuery, FilamentStockDto?>
{
    public async Task<FilamentStockDto?> Handle(GetFilamentStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await repository.GetByIdAsync(request.Id, cancellationToken);
        return stock is null ? null : FilamentStockMapper.ToDto(stock);
    }
}
