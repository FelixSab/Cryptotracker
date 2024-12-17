using Cryptotracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptotracker.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; } = null!;
    public DbSet<PriceHistory> PriceHistories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
