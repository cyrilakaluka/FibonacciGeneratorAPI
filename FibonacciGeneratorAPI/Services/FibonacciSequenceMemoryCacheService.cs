using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FibonacciGeneratorAPI.Services
{
    public class FibonacciSequenceMemoryCacheService : MemoryCacheService<string, int>, IFibonacciSequenceMemoryCacheService
    {
        public FibonacciSequenceMemoryCacheService(IMemoryCache memoryCache) : base(memoryCache)
        {
        }
    }
}
