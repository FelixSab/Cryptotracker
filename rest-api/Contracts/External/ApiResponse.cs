namespace Cryptotracker.Contracts.External;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public long Timestamp { get; set; } = default;
}
