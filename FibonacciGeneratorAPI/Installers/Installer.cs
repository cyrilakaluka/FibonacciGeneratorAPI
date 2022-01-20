using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Installers
{
    public abstract class Installer : IInstaller
    {
        protected IConfiguration Configuration { get; }

        protected IServiceCollection Services { get; }

        protected Installer(IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            Services = services;
        }

        public abstract void Install();
    }
}
