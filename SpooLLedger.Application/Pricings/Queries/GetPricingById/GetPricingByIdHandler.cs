using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Queries.GetPricingById;

public class GetPricingByIdHandler(IPricingRepository repository)
    : IRequestHandler<GetPricingByIdQuery, PricingDto?>
{
    public async Task<PricingDto?> Handle(GetPricingByIdQuery request, CancellationToken cancellationToken)
    {
        var pricing = await repository.GetByIdAsync(request.Id, cancellationToken);
        return pricing is null ? null : PricingMapper.ToDto(pricing);
    }
}
