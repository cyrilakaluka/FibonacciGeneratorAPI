using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FibonacciGeneratorAPI.Services.Interfaces
{
    public interface IFibonacciGenerator
    {
        Task<IReadOnlyCollection<string>> GenerateSubsequenceAsync(int startIndex, int endIndex, bool useCache, CancellationToken token);
    }
}
