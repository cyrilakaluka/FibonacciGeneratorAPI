using System.Collections.Generic;
using System.Linq;

namespace FibonacciGeneratorAPI.DTOs.Response
{
    public class FibonacciSubsequenceResponse
    {
        public IReadOnlyCollection<string> Result { get; set; } = Enumerable.Empty<string>().ToList();

        public int ResultCount => Result.Count;

        public ICollection<string> Errors { get; set; } = Enumerable.Empty<string>().ToList();
    }
}
