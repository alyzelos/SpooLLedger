using System;

namespace SpooLLedger.Domain;

public class FilamentStock
{
    public int Id { get; set; }
    public string? Producer { get; set; }
    public string? Type { get; set; }
    public string? Color { get; set; }
    public int Quantity { get; set; }

}
