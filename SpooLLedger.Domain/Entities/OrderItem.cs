namespace SpooLLedger.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public string FilamentType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int CompletedQuantity { get; set; }

    public decimal TotalWeight => Item is null ? 0 : Item.Weight * Quantity;
    public decimal TotalPrice => (Item?.PricePerUnit ?? 0) * Quantity;
}
