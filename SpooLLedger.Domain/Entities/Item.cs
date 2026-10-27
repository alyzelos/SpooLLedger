namespace SpooLLedger.Domain.Entities;

public class Item
{
    public int Id { get; set; }
    public string? ProductName { get; set; } 
    public int Weight { get; set; }
    public int PrintDurationMinutes { get; set; }
    public string? Details { get; set; }

    //think of a way to establish price for the piece bassed on 
    // Time,
    // Printer Degradation, 
    // Filament quantity, 
    // a procentage of errors, and consumtion based on time
    public decimal? PricePerUnit { get; set; }

    public List<OrderItem> OrderItems { get; set; } = [];
}
