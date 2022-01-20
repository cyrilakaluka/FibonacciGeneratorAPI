using FibonacciGeneratorAPI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Fibonacci Generator API", Version = "v1" });

                // Add XML documentation
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                x.IncludeXmlComments(filePath);
            });

            return services;
        }
    }
}
