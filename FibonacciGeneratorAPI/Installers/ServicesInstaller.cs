using FibonacciGeneratorAPI.Services;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Installers
{
    public class ServicesInstaller : Installer
    {
        public ServicesInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            Services.AddScoped<IFibonacciGenerator, FibonacciGenerator>()
                    .AddScoped<IMemoryUsageMonitor, MemoryUsageMonitor>();
        }
    }
}
