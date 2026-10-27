namespace SpooLLedger.Application.Pricings.Dtos;

public class PricingDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal ElectricityCostPerKwh { get; set; }
    public decimal FilamentSpoolPrice { get; set; }
    public decimal FilamentCostPerGram { get; set; }
    public decimal PrinterMaintenanceCostPerHour { get; set; }
}
