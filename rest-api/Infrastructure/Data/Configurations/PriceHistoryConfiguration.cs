using Cryptotracker.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Cryptotracker.Infrastructure.Data.Configurations;

public class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Price)
            .HasPrecision(18, 8);

        builder.Property(p => p.MarketCap)
            .HasPrecision(18, 2);

        builder.Property(p => p.Volume)
            .HasPrecision(18, 2);

        builder.HasIndex(p => new { p.CryptoCurrencyId, p.Timestamp });
    }
}
