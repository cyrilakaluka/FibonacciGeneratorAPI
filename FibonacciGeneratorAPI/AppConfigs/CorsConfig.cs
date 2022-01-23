using System.Collections.Generic;

namespace FibonacciGeneratorAPI.AppConfigs
{
    public class CorsConfig : IAppConfig
    {
        public string ConfigName => nameof(CorsConfig);

        public IEnumerable<string> AllowedOrigins { get; set; }

        public IEnumerable<string> AllowedMethods { get; set; }

        public const string DefaultPolicy = nameof(DefaultPolicy);
    }
}
