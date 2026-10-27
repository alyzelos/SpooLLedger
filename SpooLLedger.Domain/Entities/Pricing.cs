namespace SpooLLedger.Domain.Entities;

public class Pricing
{
    public const int GramsPerSpool = 1000;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal ElectricityCostPerKwh { get; set; }
    public decimal FilamentSpoolPrice { get; set; }
    public decimal PrinterMaintenanceCostPerHour { get; set; }

    public decimal FilamentCostPerGram => FilamentSpoolPrice / GramsPerSpool;
}
