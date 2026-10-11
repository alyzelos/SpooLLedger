namespace SpooLLedger.Application.FilamentStock.Dtos;

public class FilamentStockDto
{
    public int Id { get; set; }
    public string? Producer { get; set; }
    public string? Type { get; set; }
    public string? Color { get; set; }
    public int Quantity { get; set; }
}
