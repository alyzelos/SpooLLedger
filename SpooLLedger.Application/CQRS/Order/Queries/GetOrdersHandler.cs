using AutoMapper;
using MediatR;
using SpooLLedger.Application.Abstractions;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrdersHandler(IOrderRspository orderRspository, IMapper mapper) : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderQueryResponse>>
{
    public async Task<IReadOnlyList<OrderQueryResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRspository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<OrderQueryResponse>>(orders);
    }
}
