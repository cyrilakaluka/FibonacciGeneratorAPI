using System.Threading;
using System.Threading.Tasks;

namespace FibonacciGeneratorAPI.Services.Interfaces
{
    public interface IMemoryUsageMonitor
    {
        Task MonitorMaxMemUsageAsync(int limit, CancellationToken token, int pollingInterval = 0);

        int PrivateMemorySize { get; }
    }
}
