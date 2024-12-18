using Cryptotracker.API.Attributes;
using Cryptotracker.Core.DTOs.Requests;
using Cryptotracker.Core.DTOs.Responses;
using Cryptotracker.Core.Entities;
using Cryptotracker.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Cryptotracker.API.Utils.ErrorResponses;

namespace Cryptotracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrenciesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CryptoCurrencyListDto), StatusCodes.Status200OK)]

    public async Task<ActionResult<IEnumerable<CryptoCurrencyListDto>>> Get([FromQuery] CurrencyQueryParameters query)
    {
        var currencies = context.CryptoCurrencies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            currencies = currencies.Where(c =>
                c.Name.Contains(query.SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                c.Symbol.Contains(query.SearchTerm, StringComparison.InvariantCultureIgnoreCase));
        }

        currencies = query.SortBy?.ToLower() switch
        {
            "name" => query.IsDescending ?
                currencies.OrderByDescending(c => c.Name) :
                currencies.OrderBy(c => c.Name),
            "symbol" => query.IsDescending ?
                currencies.OrderByDescending(c => c.Symbol) :
                currencies.OrderBy(c => c.Symbol),
            "price" => query.IsDescending ?
                currencies.OrderByDescending(c => c.CurrentPrice) :
                currencies.OrderBy(c => c.CurrentPrice),
            "change" => query.IsDescending ?
                currencies.OrderByDescending(c => c.PriceChange24h) :
                currencies.OrderBy(c => c.PriceChange24h),
            _ => currencies.OrderBy(c => c.Name)
        };

        var pagedCurrencies = await currencies
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new CryptoCurrencyListDto(c))
            .ToListAsync();

        return Ok(pagedCurrencies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CryptoCurrencyDto), StatusCodes.Status200OK)]
    [ProducesProblemDetails(StatusCodes.Status400BadRequest)]
    [ProducesProblemDetails(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CryptoCurrencyDto>> Get(int id)
    {
        var currency = await context.CryptoCurrencies
            .Include(c => c.PriceHistory)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (currency == null)
            return NotFound(CreateNotFound($"Currency with the id {id} not found!"));

        return new CryptoCurrencyDto(currency);
    }
}
