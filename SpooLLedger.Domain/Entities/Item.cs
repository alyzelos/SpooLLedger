namespace SpooLLedger.Domain.Entities;

public class Item
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Weight { get; set; }
    public decimal PricePerUnit { get; set; }

    public List<OrderItem> OrderItems { get; set; } = [];
}
