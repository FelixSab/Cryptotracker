using System.Net.Http.Json;
using Cryptotracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Cryptotracker.Core.DTOs.CoinApi;

namespace CryptoTracker.Infrastructure.Services;

public class CoinApiService(HttpClient httpClient) : ICoinApiService
{
    public async Task<IEnumerable<CoinApiAssetDto>> GetAllAssetsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetDto[]>>("assets");
        return response?.Data ?? [];
    }

    public async Task<CoinApiAssetDto?> GetAssetByIdAsync(string id)
    {
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetDto>>($"assets/{id}");
        return response?.Data;
    }

    public async Task<IEnumerable<CoinApiAssetHistoryDto>> GetAssetHistoryAsync(string id, string interval, decimal start = 0, decimal end = 0)
    {
        var query = new Dictionary<string, string?> { ["interval"] = interval };

        if (start != 0) query.Add("start", start.ToString());
        if (end != 0) query.Add("end", end.ToString());

        var url = QueryHelpers.AddQueryString($"assets/{id}/history", query);
        var response = await httpClient.GetFromJsonAsync<ApiResponse<CoinApiAssetHistoryDto[]>>(url);
        return response?.Data ?? [];
    }

}