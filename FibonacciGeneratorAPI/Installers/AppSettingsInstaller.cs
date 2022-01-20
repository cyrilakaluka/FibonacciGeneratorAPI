using FibonacciGeneratorAPI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FibonacciGeneratorAPI.AppConfig;

namespace FibonacciGeneratorAPI.Installers
{
    public class AppSettingsInstaller : Installer
    {
        public AppSettingsInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            var appSettings = typeof(Startup).Assembly.ExportedTypes.Where(t => t.IsConcreteTypeOf(typeof(IAppConfig)))
                .Select(t => (t, Activator.CreateInstance(t) as IAppConfig));

            foreach (var (type, instance) in appSettings)
            {
                Services.AddSingleton(type, instance);
                Configuration.Bind(instance.ConfigName, instance);
            }
        }
    }
}
