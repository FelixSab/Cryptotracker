using Cryptotracker.API.Attributes;
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
    public async Task<ActionResult<IEnumerable<CryptoCurrencyListDto>>> Get()
    {
        var currencies = await context.CryptoCurrencies
            .Select(c => new CryptoCurrencyListDto(c))
            .ToListAsync();

        return Ok(currencies);
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
