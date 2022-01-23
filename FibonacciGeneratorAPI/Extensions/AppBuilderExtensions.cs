using FibonacciGeneratorAPI.AppConfigs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FibonacciGeneratorAPI.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app)
        {
            // Configure Swagger
            var swaggerConfig = app.ApplicationServices.GetService<SwaggerConfig>();
            app.UseSwagger(options => options.RouteTemplate = swaggerConfig?.JsonRoute);
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = string.Empty;
                x.SwaggerEndpoint($"{swaggerConfig?.UiEndpoint}", swaggerConfig?.Description);
            });

            return app;
        }
    }
}
