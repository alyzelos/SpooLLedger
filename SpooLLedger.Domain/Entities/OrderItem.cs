using System;

namespace SpooLLedger.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid OrderId { get; set; }

    // Core item information
    public string ProductName { get; set; } = string.Empty;
    public int Weight { get; set; }
    public decimal PricePerUnit { get; set; }
    public int Quantity { get; set; }

    // Computed property for the total cost of this specific item line
    public decimal TotalWeight => Weight * Quantity;
    public decimal TotalPrice => PricePerUnit * Quantity;
}
