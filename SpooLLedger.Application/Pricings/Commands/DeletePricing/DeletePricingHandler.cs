using MediatR;
using SpooLLedger.Application.Abstractions;

namespace SpooLLedger.Application.Pricings.Commands;

public class DeletePricingHandler(IPricingRepository repository)
    : IRequestHandler<DeletePricingCommand, bool>
{
    public async Task<bool> Handle(DeletePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (pricing is null)
            return false;

        await repository.DeleteAsync(pricing, cancellationToken);
        return true;
    }
}
