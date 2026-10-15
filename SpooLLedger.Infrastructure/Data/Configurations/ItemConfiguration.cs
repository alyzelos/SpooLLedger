using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(item => item.ProductName).IsRequired().HasMaxLength(100);
        builder.Property(item => item.PricePerUnit).HasPrecision(18, 2);

        builder.HasMany(item => item.OrderItems)
            .WithOne(orderItem => orderItem.Item)
            .HasForeignKey(orderItem => orderItem.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
