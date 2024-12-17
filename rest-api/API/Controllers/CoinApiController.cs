using Microsoft.AspNetCore.Mvc;
using Cryptotracker.API.Attributes;
using static Cryptotracker.API.Utils.ErrorResponses;
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

    //[HttpPost]
    //[ProducesResponseType(typeof(ChatCreateResponseDto), StatusCodes.Status200OK)]
    //[ProducesProblemDetails(StatusCodes.Status400BadRequest)]
    //[ProducesProblemDetails(StatusCodes.Status404NotFound)]
    //public ActionResult<ChatCreateResponseDto> CreateChat([FromBody] ChatCreateRequestDto request)
    //{
    //    if (!_chatbotClient.BotExists(request.BotId))
    //    {
    //        return NotFound(CreateNotFound($"No bot found with the provided botId: {request.BotId}"));
    //    }

    //    var response = _chatbotClient.CreateChat(request.BotId! /* validated through data annotations */);

    //    return Ok(response);
    //}

}
