using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.AppConfig;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FibonacciGeneratorAPI.Services
{
    public class FibonacciGenerator : IFibonacciGenerator
    {
        private readonly IFibonacciSequenceMemoryCacheService _cacheService;
        private readonly CacheConfig _cacheConfig;

        public FibonacciGenerator(IFibonacciSequenceMemoryCacheService cacheService, CacheConfig cacheConfig)
        {
            _cacheService = cacheService;
            _cacheConfig = cacheConfig;
        }

        public async Task<IReadOnlyCollection<int>> GenerateSubsequenceAsync(int startIndex, int endIndex, bool useCache, CancellationToken token)
        {
            var output = new Dictionary<int, int>();

            for (var i = startIndex; i <= endIndex && !token.IsCancellationRequested; i++)
            {
                // if useCache is set to true and the cached item exists, add the fibonacci number from cache then continue to the next index
                int? cachedItem;
                if (useCache && (cachedItem = await _cacheService.GetAsync(i)).HasValue)
                {
                    output.Add(i, cachedItem.Value);
                    continue;
                }

                // Otherwise add the fibonacci number after computing it
                try
                {
                    var fibonacci = await GetFibonacciAsync(i, token);
                    output.Add(i, fibonacci);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            return output.Values;
        }

        private async Task<int> GetFibonacciAsync(int n, CancellationToken token)
        {
            // Throw if task cancellation was requested
            token.ThrowIfCancellationRequested();

            int output;

            if (n < 2)
            {
                output = n;
            }
            else
            {
                var taskA = GetFibonacciAsync(n - 1, token);
                var taskB = GetFibonacciAsync(n - 2, token);
                await Task.WhenAll(taskA, taskB);

                output = taskA.Result + taskB.Result;
            }

            _cacheService.Set(n, output, new MemoryCacheEntryOptions { SlidingExpiration = _cacheConfig.SlidingExpiration });
            return output;
        }
    }
}
