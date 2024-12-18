namespace Cryptotracker.Core.DTOs.CoinApi;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public decimal Timestamp { get; set; } = default;
}
