using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.FilamentStock.Commands;

public class DeleteFilamentStockHandler(IFilamentStockRepository repository)
    : IRequestHandler<DeleteFilamentStockCommand, bool>
{
    public async Task<bool> Handle(DeleteFilamentStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (stock is null)
            return false;

        await repository.DeleteAsync(stock, cancellationToken);
        return true;
    }
}
