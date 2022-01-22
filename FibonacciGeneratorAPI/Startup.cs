using FibonacciGeneratorAPI.AppConfig;
using FibonacciGeneratorAPI.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ExceptionHandlerMiddleware = FibonacciGeneratorAPI.MiddleWares.ExceptionHandlerMiddleware;

namespace FibonacciGeneratorAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(configuration)
                         .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configure Swagger
                var swaggerConfig = app.ApplicationServices.GetService<SwaggerConfig>();
                app.UseSwagger(options => options.RouteTemplate = swaggerConfig?.JsonRoute);
                app.UseSwaggerUI(x =>
                {
                    x.RoutePrefix = string.Empty;
                    x.SwaggerEndpoint($"{swaggerConfig?.UiEndpoint}", swaggerConfig?.Description);
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors(CorsConfig.DefaultPolicy);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
