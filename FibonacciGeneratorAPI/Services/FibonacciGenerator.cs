using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.AppConfig;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FibonacciGeneratorAPI.Services
{
    public class FibonacciGenerator : IFibonacciGenerator
    {
        private readonly IFibonacciSequenceMemoryCacheService _cacheService;
        private readonly ILogger<FibonacciGenerator> _logger;
        private readonly CacheConfig _cacheConfig;

        public FibonacciGenerator(IFibonacciSequenceMemoryCacheService cacheService, 
                                  ILogger<FibonacciGenerator> logger, 
                                  CacheConfig cacheConfig)
        {
            _cacheService = cacheService;
            _logger = logger;
            _cacheConfig = cacheConfig;
        }

        public async Task<IReadOnlyCollection<string>> GenerateSubsequenceAsync(int startIndex, int endIndex, bool useCache, CancellationToken token)
        {
            var output = new List<string>();

            const int numParallelBackgroundTasks = 4;

            _logger.LogInformation("Generating fibonacci subsequence. Number of background tasks: {0}", numParallelBackgroundTasks);

            for (var i = startIndex; i <= endIndex && !token.IsCancellationRequested; i += numParallelBackgroundTasks)
            {
                var tasks = new List<Task<string>>();

                for (var j = i; j < i + numParallelBackgroundTasks; j++)
                {
                    if (j > endIndex) continue;

                    var n = j;
                    tasks.Add(Task.Run(() => GetFibonacci(n, useCache, token), token));
                }

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                tasks.ForEach(task => output.Add(task.Result));
            }

            _logger.LogInformation("Generating fibonacci subsequence ended. Returning output.");

            return output;
        }

        private async Task<string> GetFibonacci(int n, bool useCache, CancellationToken token)
        {
            string cachedItem;
            if (useCache && !string.IsNullOrEmpty(cachedItem = await _cacheService.GetAsync(n)))
            {
                return cachedItem;
            }

            var output =  Fibonacci(n, token).ToString();

            _cacheService.Set(n, output, new MemoryCacheEntryOptions { SlidingExpiration = _cacheConfig.SlidingExpiration });

            return output;
        }

        private static BigInteger Fibonacci(int n, CancellationToken token)
        {
            BigInteger output = 0;

            if (n < 2)
                output = n;
            else
            {
                BigInteger a = 0;
                BigInteger b = 1;

                for (var i = 2; i <= n; i++)
                {
                    token.ThrowIfCancellationRequested();

                    output = a + b;
                    a = b;
                    b = output;
                }
            }

            return output;
        }
    }
}
