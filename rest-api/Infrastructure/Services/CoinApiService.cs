using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Cryptotracker.Core.Interfaces.Services;
using Cryptotracker.Contracts.External;
using NpgsqlTypes;

namespace CryptoTracker.Infrastructure.Services;

public class CoinApiService(HttpClient httpClient) : ICoinApiService
{
    public async Task<IEnumerable<CoinApiAssetDto>> GetAllAssetsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetDto[]>>("/assets");
        return response?.Data ?? [];
    }

    public async Task<CoinApiAssetDto?> GetAssetByIdAsync(string id)
    {
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetDto>>($"/assets/{id}");
        return response?.Data;
    }

    public async Task<IEnumerable<CoinApiAssetHistoryDto>> GetAssetHistoryAsync(string id, string interval, long start = 0, long end = 0)
    {
        var url = $"/assets/{id}/history?interval={interval}{(start != 0 ? $";start={start}" : "")}{(end != 0 ? $";end={end}" : "")}";
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetHistoryDto[]>>(url);
        return response?.Data ?? [];
    }

}