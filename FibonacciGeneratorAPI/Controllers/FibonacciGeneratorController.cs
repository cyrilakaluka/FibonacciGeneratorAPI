using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciGeneratorAPI.Controllers
{
    public class FibonacciGeneratorController : ControllerBase
    {
        [HttpGet(Routes.FibonacciSubsequence)]
        public async Task<IActionResult>GetFibonacciSubsequence()
        {
            return await Task.Run(Ok);
        }
    }
}
