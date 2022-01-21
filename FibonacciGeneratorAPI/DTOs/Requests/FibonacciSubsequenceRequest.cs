using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FibonacciGeneratorAPI.DTOs.Requests
{
    public class FibonacciSubsequenceRequest
    {
        /// <summary>
        /// The start index of the Fibonacci subsequence. Must be 0 or greater and also less than or equal to the end index.
        /// </summary>
        [Required]
        [Range(0, uint.MaxValue, ErrorMessage = "Start index cannot be negative")]
        public int StartIndex { get; set; }

        /// <summary>
        /// The end index of the Fibonacci subsequence. Must 0 or greater and also greater than or equal to the start index.
        /// </summary>
        [Required]
        [Range(0, uint.MaxValue, ErrorMessage = "End index cannot be negative")]
        public int EndIndex { get; set; }

        /// <summary>
        /// Indicates whether to use caching or not to speed up the Fibonacci generation process.
        /// </summary>
        [DefaultValue(false)]
        public bool UseCache { get; set; } = false;

        /// <summary>
        /// The time in milliseconds(ms) that the Fibonacci sequence request is allowed to execute before timing out.
        /// </summary>
        [DefaultValue(int.MaxValue)]
        [Range(200, int.MaxValue)]
        public int ExecutionTimeout { get; set; } = int.MaxValue;

        /// <summary>
        /// The maximum amount of memory in MegaBytes(MB) that the program is allowed to use.
        /// </summary>
        [DefaultValue(int.MaxValue)]
        [Range(200, int.MaxValue)]
        public int MaxAllowedMemoryUsage { get; set; } = int.MaxValue;
    }
}
