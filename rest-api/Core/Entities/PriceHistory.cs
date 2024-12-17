namespace Cryptotracker.Core.Entities;

public class PriceHistory
{
    public int Id { get; set; }
    public int CryptoCurrencyId { get; set; }
    public decimal Price { get; set; }
    public decimal MarketCap { get; set; }
    public decimal Volume { get; set; }
    public DateTime Timestamp { get; set; }
    public virtual CryptoCurrency CryptoCurrency { get; set; } = null!;
}
