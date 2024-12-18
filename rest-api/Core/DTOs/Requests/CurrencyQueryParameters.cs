
namespace Cryptotracker.Core.DTOs.Requests;

public class CurrencyQueryParameters
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } = "name";
    public bool IsDescending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}
