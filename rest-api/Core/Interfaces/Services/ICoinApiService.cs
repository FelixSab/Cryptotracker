using Cryptotracker.Core.DTOs.CoinApi;

namespace Cryptotracker.Core.Interfaces.Services;

public interface ICoinApiService
{
    Task<IEnumerable<CoinApiAssetDto>> GetAllAssetsAsync();
    Task<CoinApiAssetDto?> GetAssetByIdAsync(string id);
    Task<IEnumerable<CoinApiAssetHistoryDto>> GetAssetHistoryAsync(string id, string interval, decimal start = 0, decimal end = 0);
}
