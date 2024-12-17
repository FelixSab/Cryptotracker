using Microsoft.AspNetCore.Mvc;
using Cryptotracker.Core.Interfaces.Services;
using Cryptotracker.Contracts.External;

namespace Cryptotracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CoinApiController(ICoinApiService _coinApiService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<CoinApiAssetDto[]>> GetAssets()
    {
        var response = await _coinApiService.GetAllAssetsAsync();
        return Ok(response);
    }

    [HttpGet("{assetId}/completions")]
    public async Task<ActionResult<CoinApiAssetHistoryDto[]>> GetAssetHistory(
        [FromRoute] string assetId,
        [FromQuery] string interval,
        [FromQuery] long start,
        [FromQuery] long end
    )
    {
        var response = await _coinApiService.GetAssetHistoryAsync(assetId, interval, start, end);
        return Ok(response);
    }
}
