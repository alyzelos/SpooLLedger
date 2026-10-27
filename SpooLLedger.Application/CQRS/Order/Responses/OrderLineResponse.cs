namespace SpooLLedger.Application.CQRS.Order.Responses;

public class OrderLineResponse
{
    public int ItemId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string FilamentType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Weight { get; set; }
    public int PrintDurationMinutes { get; set; }
    public decimal? PricePerUnit { get; set; }
    public int Quantity { get; set; }
    public int CompletedQuantity { get; set; }
    public decimal TotalWeight { get; set; }
    public decimal TotalPrice { get; set; }
}
