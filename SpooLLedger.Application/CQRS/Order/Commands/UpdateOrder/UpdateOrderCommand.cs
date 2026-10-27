using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<UpdateOrderResponse>
{
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; init; }
    public string? Status { get; set; }
    public string PricingName { get; set; } = "Standard";
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = [];
}
