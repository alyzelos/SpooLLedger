using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.FilamentStock.Dtos;

namespace SpooLLedger.Application.FilamentStock.Commands;

public class CreateFilamentStockHandler(IFilamentStockRepository repository)
    : IRequestHandler<CreateFilamentStockCommand, FilamentStockDto>
{
    public async Task<FilamentStockDto> Handle(CreateFilamentStockCommand request, CancellationToken cancellationToken)
    {
        var stock = FilamentStockMapper.ToEntity(request);
        await repository.AddAsync(stock, cancellationToken);
        return FilamentStockMapper.ToDto(stock);
    }
}
