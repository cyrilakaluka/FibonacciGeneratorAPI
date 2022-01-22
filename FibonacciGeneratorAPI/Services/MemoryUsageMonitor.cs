using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FibonacciGeneratorAPI.Services
{
    public class MemoryUsageMonitor : IMemoryUsageMonitor
    {
        private readonly ILogger<MemoryUsageMonitor> _logger;

        public MemoryUsageMonitor(ILogger<MemoryUsageMonitor> logger)
        {
            _logger = logger;
        }

        public async Task MonitorMaxMemUsageAsync(int limit, int pollingInterval, CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(pollingInterval, token);

                using var proc = Process.GetCurrentProcess();
                var memoryInUse = proc.PrivateMemorySize64 >> 20;
                _logger.LogDebug($"Memory in use: {memoryInUse}, Limit: {limit}");

                if (memoryInUse >= limit)
                    return;
            }
        }
    }
}
