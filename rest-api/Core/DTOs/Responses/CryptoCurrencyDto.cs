using Cryptotracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Core.DTOs.Responses;

public record CryptoCurrencyDto
{
    public int Id { get; init; }
    public string Symbol { get; init; }
    public string Name { get; init; }
    public decimal CurrentPrice { get; init; }
    public decimal PriceChange24h { get; init; }
    public DateTime LastUpdated { get; init; }
    public ICollection<PriceHistoryDto> PriceHistory { get; init; }

    public CryptoCurrencyDto(CryptoCurrency entity)
    {
        Id = entity.Id;
        Symbol = entity.Symbol;
        Name = entity.Name;
        CurrentPrice = entity.CurrentPrice;
        PriceChange24h = entity.PriceChange24h;
        LastUpdated = entity.LastUpdated;
        PriceHistory = entity.PriceHistory.Select(p => new PriceHistoryDto(p)).ToList();
    }
}
