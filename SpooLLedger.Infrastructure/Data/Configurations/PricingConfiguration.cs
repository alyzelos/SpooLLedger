using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.Configurations;

public class PricingConfiguration : IEntityTypeConfiguration<Pricing>
{
    public void Configure(EntityTypeBuilder<Pricing> builder)
    {
        builder.HasKey(pricing => pricing.Id);
        builder.Property(pricing => pricing.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(pricing => pricing.Name).IsUnique();
        builder.Property(pricing => pricing.ElectricityCostPerKwh).HasPrecision(18, 4);
        builder.Property(pricing => pricing.FilamentSpoolPrice).HasPrecision(18, 2);
        builder.Property(pricing => pricing.PrinterMaintenanceCostPerHour).HasPrecision(18, 4);
        builder.Ignore(pricing => pricing.FilamentCostPerGram);
    }
}
