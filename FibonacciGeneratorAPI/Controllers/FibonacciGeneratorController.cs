using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciGeneratorAPI.Controllers
{
    public class FibonacciGeneratorController : ControllerBase
    {
        /// <summary>
        /// Generates a subsequence from a sequence of Fibonacci numbers according to the start and end index parameters.
        /// </summary>
        /// <returns>An awaitable task of type <see cref="IActionResult"/>.</returns>
        [HttpGet(Routes.FibonacciSubsequence)]
        public async Task<IActionResult>GetFibonacciSubsequence()
        {
            return await Task.Run(Ok);
        }
    }
}
