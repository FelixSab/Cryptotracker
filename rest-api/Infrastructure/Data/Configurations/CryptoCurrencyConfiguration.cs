using Cryptotracker.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Cryptotracker.Infrastructure.Data.Configurations;

public class CryptoCurrencyConfiguration : IEntityTypeConfiguration<CryptoCurrency>
{
    public void Configure(EntityTypeBuilder<CryptoCurrency> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Symbol)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CurrentPrice)
            .HasPrecision(18, 8);

        builder.Property(c => c.MarketCap)
            .HasPrecision(18, 2);

        builder.HasMany(c => c.PriceHistory)
            .WithOne(p => p.CryptoCurrency)
            .HasForeignKey(p => p.CryptoCurrencyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.Symbol)
            .IsUnique();
    }
}

