using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Queries;

public class GetOrderByOrderNumberQuery : IRequest<OrderQueryResponse>
{
    public GetOrderByOrderNumberQuery(int orderNumber)
    {
        OrderNumber = orderNumber;
    }
    public int OrderNumber { get; set; }
}
