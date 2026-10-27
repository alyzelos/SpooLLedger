using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpooLLedger.Domain.Entities;

namespace SpooLLedger.Infrastructure.Data.Configurations;

public class FilamentTypeConfiguration : IEntityTypeConfiguration<FilamentType>
{
    public void Configure(EntityTypeBuilder<FilamentType> builder)
    {
        builder.HasKey(type => type.Id);
        builder.Property(type => type.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(type => type.Name).IsUnique();
    }
}
