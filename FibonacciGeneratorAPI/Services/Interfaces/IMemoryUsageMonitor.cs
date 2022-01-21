using System.Threading;
using System.Threading.Tasks;

namespace FibonacciGeneratorAPI.Services.Interfaces
{
    public interface IMemoryUsageMonitor
    {
        Task MonitorMaxMemUsageAsync(int limit, int pollingInterval, CancellationToken token);
    }
}
