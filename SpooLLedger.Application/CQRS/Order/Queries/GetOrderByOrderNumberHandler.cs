using System;
using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrderByOrderNumberHandler(
    IOrderRspository orderRspository,
    IMapper mapper) : IRequestHandler<GetOrderByOrderNumberQuery, OrderQueryResponse>
{
    public async Task<OrderQueryResponse> Handle(GetOrderByOrderNumberQuery query, CancellationToken cancellationToken)
    { 
        var order = await orderRspository.GetByOrderNumber(query.OrderNumber, cancellationToken);
        return mapper.Map<OrderQueryResponse>(order);
    }
}
