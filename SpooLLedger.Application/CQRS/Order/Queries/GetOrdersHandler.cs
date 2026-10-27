using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrdersHandler(
    IOrderRepository orderRspository,
    IPricingRepository pricings,
    IItemPriceCalculator calculator,
    IMapper mapper) : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderQueryResponse>>
{
    public async Task<IReadOnlyList<OrderQueryResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRspository.GetAllAsync(cancellationToken);
        var response = mapper.Map<IReadOnlyList<OrderQueryResponse>>(orders);
        await OrderPrices.Apply(response, pricings, calculator, cancellationToken);
        return response;
    }
}
