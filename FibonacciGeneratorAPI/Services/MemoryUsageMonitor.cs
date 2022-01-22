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

        public int PrivateMemorySize  {
            get
            {
                using var proc = Process.GetCurrentProcess();
                return (int)proc.PrivateMemorySize64 >> 20;
            }
        }

        public async Task MonitorMaxMemUsageAsync(int limit, CancellationToken token, int pollingInterval = 0)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(pollingInterval, token);

                if (PrivateMemorySize >= limit)
                    return;
            }
        }
    }
}
