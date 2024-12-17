namespace Cryptotracker.Core.Entities;

public class CryptoCurrency
{
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal PriceChange24h { get; set; }
    public decimal MarketCap { get; set; }
    public decimal Volume24h { get; set; }
    public decimal CirculatingSupply { get; set; }
    public decimal? MaxSupply { get; set; }
    public DateTime LastUpdated { get; set; }
    public virtual ICollection<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
}
