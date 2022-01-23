using System;

namespace FibonacciGeneratorAPI.AppConfigs
{
    public class CacheConfig : IAppConfig
    {
        public string ConfigName => nameof(CacheConfig);

        /// <summary>
        /// Gets or sets the time period a cache entry can be inactive after which it is invalidated
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of time between successive scans for expired cache entries.
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; }
    }
}
