using System.Collections.Generic;
using System.Linq;

namespace FibonacciGeneratorAPI.DTOs.Responses
{
    public class FibonacciSubsequenceResponse
    {
        /// <summary>
        /// A list of the string representation of each number in the Fibonacci subsequence
        /// </summary>
        /// <example>["0", "1", "1", "2", "3", "5"]</example>
        public IReadOnlyCollection<string> Result { get; set; } = Enumerable.Empty<string>().ToList();

        /// <summary>
        /// The count of the items returned in the result
        /// </summary>
        /// <example>6</example>
        public int ResultCount => Result.Count;

        /// <summary>
        /// A list of the errors encountered while generating the Fibonacci subsequence.
        /// </summary>
        /// <example>["Timeout error occurred."]</example>
        public ICollection<string> Errors { get; set; } = Enumerable.Empty<string>().ToList();
    }
}
