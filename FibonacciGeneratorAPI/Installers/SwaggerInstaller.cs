using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FibonacciGeneratorAPI.Installers
{
    public class SwaggerInstaller : Installer
    {
        public SwaggerInstaller(IServiceCollection services, IConfiguration configuration) : base(services, configuration)
        {
        }

        public override void Install()
        {
            Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Fibonacci Generator API", Version = "v1" });

                // Add XML documentation
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                x.IncludeXmlComments(filePath);
            });
        }
    }
}
