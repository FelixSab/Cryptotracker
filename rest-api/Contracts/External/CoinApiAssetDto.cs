namespace Cryptotracker.Contracts.External;

public class CoinApiAssetDto
{
    public string? Id { get; set; } = null;
    public string? Rank { get; set; } = null;
    public string? Symbol { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? Supply { get; set; } = null;
    public string? MaxSupply { get; set; } = null;
    public string? MarketCapUsd { get; set; } = null;
    public string? VolumeUsd24Hr { get; set; } = null;
    public string? PriceUsd { get; set; } = null;
    public string? ChangePercent24Hr { get; set; } = null;
    public string? Vwap24Hr { get; set; } = null;
}
