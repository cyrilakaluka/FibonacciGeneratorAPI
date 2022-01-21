using FibonacciGeneratorAPI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FibonacciGeneratorAPI.Installers.Interfaces;

namespace FibonacciGeneratorAPI.Installers
{
    public static class InstallerExtension
    {
        public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(t => t.IsConcreteTypeOf(typeof(IInstaller)))
                .Select(t => Activator.CreateInstance(t, services, configuration))
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(i => i.Install());
        }
    }
}
