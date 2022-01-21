using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.Services.Interfaces;

namespace FibonacciGeneratorAPI.Services
{
    public class MemoryUsageMonitor : IMemoryUsageMonitor
    {
        public async Task MonitorMaxMemUsageAsync(int limit, int pollingInterval, CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(pollingInterval, token);

                using var proc = Process.GetCurrentProcess();
                var memoryInUse = proc.PrivateMemorySize64 >> 20;

                if (memoryInUse >= limit)
                    return;
            }
        }
    }
}
