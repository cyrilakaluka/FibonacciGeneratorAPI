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
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fibonacci Generator API", 
                    Version = "v1",
                    Description = "An API to generate a subsequence from a Fibonacci sequence",
                    Contact = new OpenApiContact
                    {
                        Name = "Chibueze Cyril Akaluka",
                        Email = "akalukacyril@gmail.com",
                        Url = new Uri("https://github.com/cyrilakaluka")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/cyrilakaluka/FibonacciGeneratorAPI/blob/master/LICENSE")
                    }
                });

                // Add XML documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if(File.Exists(xmlPath))
                    x.IncludeXmlComments(xmlPath);

                x.DescribeAllParametersInCamelCase();
                x.OrderActionsBy(o => o.RelativePath);
            });
        }
    }
}
