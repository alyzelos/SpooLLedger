namespace SpooLLedger.Application.CQRS.Order;

public class OrderItemRequest
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
