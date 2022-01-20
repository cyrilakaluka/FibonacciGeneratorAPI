namespace FibonacciGeneratorAPI.AppConfig
{
    public class SwaggerConfig : IAppConfig
    {
        public string ConfigName => nameof(SwaggerConfig);

        public string JsonRoute { get; set; }

        public string Description { get; set; }

        public string UiEndpoint { get; set; }
    }
}
