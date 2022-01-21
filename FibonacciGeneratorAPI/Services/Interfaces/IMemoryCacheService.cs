using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FibonacciGeneratorAPI.Services.Interfaces
{
    public interface IMemoryCacheService<T, in TKey>
    {
        Task<T> GetAsync(TKey key);

        void Set(TKey key, T entry, MemoryCacheEntryOptions options = null);

        void Remove(TKey key);
    }
}
