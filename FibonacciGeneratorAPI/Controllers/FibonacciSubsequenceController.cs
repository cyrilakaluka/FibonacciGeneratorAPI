using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.DTOs.Requests;
using FibonacciGeneratorAPI.DTOs.Response;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciGeneratorAPI.Controllers
{
    [ApiController]
    public class FibonacciSubsequenceController : ControllerBase
    {
        private readonly IFibonacciGenerator _fibonacciGenerator;
        private readonly IMemoryUsageMonitor _memoryUsageMonitor;

        public FibonacciSubsequenceController(IFibonacciGenerator fibonacciGenerator, IMemoryUsageMonitor memoryUsageMonitor)
        {
            _fibonacciGenerator = fibonacciGenerator;
            _memoryUsageMonitor = memoryUsageMonitor;
        }

        /// <summary>
        /// Generates a subsequence from a sequence of Fibonacci numbers according to the start and end index parameters.
        /// </summary>
        /// <returns>An awaitable task of type <see cref="IActionResult"/>.</returns>
        [HttpGet(Routes.GetFibonacciSubsequence)]
        public async Task<IActionResult>GetFibonacciSubsequence([FromQuery] FibonacciSubsequenceRequest request)
        {
            #region Input Validation

            // Validate the request
            if (request.StartIndex > request.EndIndex)
            {
                ModelState.AddModelError(nameof(request.StartIndex), "The start index cannot be greater than the end index.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #endregion

            #region Background Tasks Creation

            // Create the cancellation token source
            using var source = new CancellationTokenSource();

            // Assign the background tasks that should execute on the thread pool
            var executionTimeoutTask = Task.Delay(request.ExecutionTimeout, source.Token);

            var fibonacciGenerationTask = Task.Run(() => _fibonacciGenerator.GenerateSubsequenceAsync(request.StartIndex,
                                                       request.EndIndex,
                                                       request.UseCache,
                                                       source.Token), source.Token);

            var maxMemUsageMonitoringTask = Task.Run(() => _memoryUsageMonitor.MonitorMaxMemUsageAsync(request.MaxAllowedMemoryUsage,
                                                         500,
                                                         source.Token), source.Token);

            #endregion

            #region Asynchronous Wait

            try
            {
                // Wait for any of the tasks to complete
                await Task.WhenAny(fibonacciGenerationTask, executionTimeoutTask, maxMemUsageMonitoringTask);
                // Request for cancellation of all tasks
                source.Cancel();
                // Wait for all tasks to honor the cancellation request
                await Task.WhenAll(fibonacciGenerationTask, executionTimeoutTask, maxMemUsageMonitoringTask);
            }
            catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException) { } // Ignore task/operation cancellation exceptions

            #endregion

            #region Output Generation

            // Create the output response
            var response = new FibonacciSubsequenceResponse();

            // Add the errors if any
            if (executionTimeoutTask.IsCompletedSuccessfully)
                response.Errors.Add("Timeout error occurred.");

            if (maxMemUsageMonitoringTask.IsCompletedSuccessfully)
                response.Errors.Add("The maximum memory usage was reached.");

            // Assign the fibonacci subsequence result
            if(fibonacciGenerationTask.IsCompletedSuccessfully)
                response.Result = fibonacciGenerationTask.Result.ToList();

            return Ok(response);

            #endregion
        }
    }
}
