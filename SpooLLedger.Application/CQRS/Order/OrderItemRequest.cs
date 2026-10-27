namespace SpooLLedger.Application.CQRS.Order;

public class OrderItemRequest
{
    public int ItemId { get; set; }
    public string FilamentType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int CompletedQuantity { get; set; }
}
