using System;
using FibonacciGeneratorAPI.AppConfigs;
using FibonacciGeneratorAPI.Services;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Installers
{
    public class MemoryCacheInstaller : Installer
    {
        public MemoryCacheInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            Services.AddMemoryCache(o =>
            {
                if (TimeSpan.TryParse(Configuration.GetSection($"{nameof(CacheConfig)}:{nameof(CacheConfig.ExpirationScanFrequency)}").Value, 
                                      out var expirationScanFrequency))
                {
                    o.ExpirationScanFrequency = expirationScanFrequency;
                }
            });

            Services.AddScoped(typeof(IMemoryCacheService<,>), typeof(MemoryCacheService<,>));

            Services.AddSingleton<IFibonacciSequenceMemoryCacheService, FibonacciSequenceMemoryCacheService>();
        }
    }
}
