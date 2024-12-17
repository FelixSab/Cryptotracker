namespace Cryptotracker.Core.DTOs.CoinApi;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public long Timestamp { get; set; } = default;
}
