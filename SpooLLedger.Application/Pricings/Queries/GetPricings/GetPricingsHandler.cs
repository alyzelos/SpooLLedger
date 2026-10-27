using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.Pricings.Dtos;

namespace SpooLLedger.Application.Pricings.Queries.GetPricings;

public class GetPricingsHandler(IPricingRepository repository)
    : IRequestHandler<GetPricingsQuery, IReadOnlyList<PricingDto>>
{
    public async Task<IReadOnlyList<PricingDto>> Handle(GetPricingsQuery request, CancellationToken cancellationToken)
    {
        var pricings = await repository.GetAllAsync(cancellationToken);
        return pricings.Select(PricingMapper.ToDto).ToList();
    }
}
