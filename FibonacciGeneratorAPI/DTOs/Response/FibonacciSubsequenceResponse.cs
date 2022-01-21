using System.Collections.Generic;
using System.Linq;

namespace FibonacciGeneratorAPI.DTOs.Response
{
    public class FibonacciSubsequenceResponse
    {
        public ICollection<int> Result { get; set; } = Enumerable.Empty<int>().ToList();

        public ICollection<string> Errors { get; set; } = Enumerable.Empty<string>().ToList();
    }
}
