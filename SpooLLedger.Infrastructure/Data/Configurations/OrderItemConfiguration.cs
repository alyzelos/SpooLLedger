using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductName).IsRequired().HasMaxLength(100);
        builder.Property(i => i.PricePerUnit).HasPrecision(18, 2);
        builder.Ignore(i => i.TotalWeight);
        builder.Ignore(i => i.TotalPrice);
    }
}
