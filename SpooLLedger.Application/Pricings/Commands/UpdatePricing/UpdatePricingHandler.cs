using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Commands;

public class UpdatePricingHandler(IPricingRepository repository)
    : IRequestHandler<UpdatePricingCommand, PricingDto?>
{
    public async Task<PricingDto?> Handle(UpdatePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (pricing is null)
            return null;

        PricingMapper.Apply(request, pricing);
        await repository.UpdateAsync(pricing, cancellationToken);
        return PricingMapper.ToDto(pricing);
    }
}
