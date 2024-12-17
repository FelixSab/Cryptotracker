using Cryptotracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Core.DTOs.Responses;

public record PriceHistoryDto
{
    public decimal Price { get; init; }
    public DateTime Timestamp { get; init; }

    public PriceHistoryDto(PriceHistory entity)
    {
        Price = entity.Price;
        Timestamp = entity.Timestamp;
    }
}
