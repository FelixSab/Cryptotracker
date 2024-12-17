using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptotracker.Contracts.External;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public long Timestamp { get; set; } = default;
}
