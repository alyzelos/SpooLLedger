using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public DateTime OrderDate { get; init; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public string PricingName { get; set; } = "Standard";
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = [];
}
