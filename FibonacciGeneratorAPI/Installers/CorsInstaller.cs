using System.Linq;
using FibonacciGeneratorAPI.AppConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Installers
{
    public class CorsInstaller : Installer
    {
        public CorsInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            Services.AddCors(options =>
            {
                options.AddPolicy(CorsConfig.DefaultPolicy, builder =>
                {
                    var corsConfig = new CorsConfig();
                    Configuration.Bind(corsConfig.ConfigName, corsConfig);

                    builder.WithOrigins(corsConfig.AllowedOrigins.ToArray())
                           .WithMethods(corsConfig.AllowedMethods.ToArray())
                           .AllowAnyHeader();
                });
            });
        }
    }
}
