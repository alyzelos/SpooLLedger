namespace SpooLLedger.Application.CQRS.Items.Dtos;

public class ItemDto
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int Weight { get; set; }
    public int PrintDurationMinutes { get; set; }
    public string? Details { get; set; }
    public decimal? PricePerUnit { get; set; }
}
