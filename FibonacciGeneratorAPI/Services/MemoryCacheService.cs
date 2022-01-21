using System.Threading.Tasks;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FibonacciGeneratorAPI.Services
{
    public class MemoryCacheService<T, TKey> : IMemoryCacheService<T, TKey>
    {
        protected readonly IMemoryCache MemoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public Task<T> GetAsync(TKey key)
        {
            MemoryCache.TryGetValue(key, out T entry);

            return Task.FromResult(entry);
        }

        public void Set(TKey key, T entry, MemoryCacheEntryOptions options = null) =>
            MemoryCache.Set(key, entry, options);

        public void Remove(TKey key) => MemoryCache.Remove(key);
    }
}
