using MediatR;
using SpooLLedger.Application.CQRS.Order.Responses;

namespace SpooLLedger.Application.CQRS.Order.Commands.CreateOrderSoap;

public class CreateOrderSoapCommand : IRequest<CreateOrderSoapResponse>
{
    public DateTime OrderDate { get; init; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = [];
}
