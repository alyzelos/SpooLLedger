using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.Configurations;

public class FilamentColorConfiguration : IEntityTypeConfiguration<FilamentColor>
{
    public void Configure(EntityTypeBuilder<FilamentColor> builder)
    {
        builder.HasKey(color => color.Id);
        builder.Property(color => color.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(color => color.Name).IsUnique();
    }
}
