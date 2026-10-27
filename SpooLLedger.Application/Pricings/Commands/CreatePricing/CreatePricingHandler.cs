using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Commands;

public class CreatePricingHandler(IPricingRepository repository)
    : IRequestHandler<CreatePricingCommand, PricingDto>
{
    public async Task<PricingDto> Handle(CreatePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = PricingMapper.ToEntity(request);
        await repository.AddAsync(pricing, cancellationToken);
        return PricingMapper.ToDto(pricing);
    }
}
