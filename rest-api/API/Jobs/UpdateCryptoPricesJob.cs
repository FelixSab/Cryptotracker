using Cryptotracker.Core.Entities;
using Cryptotracker.Core.Interfaces.Services;
using Cryptotracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Cryptotracker.API.Jobs;



[DisallowConcurrentExecution]
public class UpdateCryptoPricesJob : IJob
{
    private readonly ICoinApiService _coinApiService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UpdateCryptoPricesJob> _logger;

    public UpdateCryptoPricesJob(
        ICoinApiService coinApiService,
        AppDbContext dbContext,
        ILogger<UpdateCryptoPricesJob> logger)
    {
        _coinApiService = coinApiService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Starting crypto price update job at: {time}", DateTimeOffset.Now);

            // Fetch latest prices from API
            var assets = await _coinApiService.GetAllAssetsAsync();

            foreach (var asset in assets)
            {
                var crypto = await _dbContext.CryptoCurrencies
                    .FirstOrDefaultAsync(c => c.Symbol == asset.Symbol);

                if (crypto == null)
                {
                    // Add new cryptocurrency
                    crypto = new CryptoCurrency
                    {
                        Symbol = asset.Symbol,
                        Name = asset.Name
                    };
                    _dbContext.CryptoCurrencies.Add(crypto);
                }

                // Update prices
                crypto.CurrentPrice = decimal.Parse(asset.PriceUsd);
                crypto.PriceChange24h = decimal.Parse(asset.ChangePercent24Hr);
                crypto.LastUpdated = DateTime.UtcNow;

                // Add price history
                var priceHistory = new PriceHistory
                {
                    CryptoCurrency = crypto,
                    Price = decimal.Parse(asset.PriceUsd),
                    Timestamp = DateTime.UtcNow
                };
                _dbContext.PriceHistories.Add(priceHistory);
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Completed crypto price update job at: {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating crypto prices");
            throw;
        }
    }
}
