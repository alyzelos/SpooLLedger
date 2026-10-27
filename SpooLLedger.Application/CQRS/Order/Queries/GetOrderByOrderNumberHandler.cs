using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Application.Pricings;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrderByOrderNumberHandler(
    IOrderRepository orderRspository,
    IPricingRepository pricings,
    IItemPriceCalculator calculator,
    IMapper mapper) : IRequestHandler<GetOrderByOrderNumberQuery, OrderQueryResponse>
{
    public async Task<OrderQueryResponse> Handle(GetOrderByOrderNumberQuery query, CancellationToken cancellationToken)
    { 
        var order = await orderRspository.GetByOrderNumber(query.OrderNumber, cancellationToken);
        var response = mapper.Map<OrderQueryResponse>(order);
        await OrderPrices.Apply([response], pricings, calculator, cancellationToken);
        return response;
    }
}
