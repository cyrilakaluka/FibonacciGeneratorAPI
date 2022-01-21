using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Installers
{
    public class MvcInstaller : Installer
    {
        public MvcInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            Services.AddControllers();
            Services.AddMvc().AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
        }
    }
}
