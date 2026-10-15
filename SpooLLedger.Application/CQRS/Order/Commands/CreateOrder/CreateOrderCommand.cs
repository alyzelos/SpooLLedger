using System;
using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public DateTime OrderDate { get; init; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public Guid CustomerId { get; set; }
}
