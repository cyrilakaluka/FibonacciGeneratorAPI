using System;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.DTOs.Requests;
using FibonacciGeneratorAPI.DTOs.Responses;
using FibonacciGeneratorAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FibonacciGeneratorAPI.Controllers
{
    [ApiController]
    public class FibonacciSubsequenceController : ControllerBase
    {
        private readonly IFibonacciGenerator _fibonacciGenerator;
        private readonly IMemoryUsageMonitor _memoryUsageMonitor;
        private readonly ILogger<FibonacciSubsequenceController> _logger;

        public FibonacciSubsequenceController(IFibonacciGenerator fibonacciGenerator, 
                                              IMemoryUsageMonitor memoryUsageMonitor,
                                              ILogger<FibonacciSubsequenceController> logger)
        {
            _fibonacciGenerator = fibonacciGenerator;
            _memoryUsageMonitor = memoryUsageMonitor;
            _logger = logger;
        }

        /// <summary>
        /// Generates a subsequence from a sequence of Fibonacci numbers according to the start and end index parameters.
        /// </summary>
        /// <returns>An awaitable task of type <see cref="IActionResult"/>.</returns>
        [HttpGet(Routes.GetFibonacciSubsequence)]
        public async Task<IActionResult>GetFibonacciSubsequence([FromQuery] FibonacciSubsequenceRequest request)
        {
            // Create the output response
            var response = new FibonacciSubsequenceResponse();

            #region Input Validation

            // Validate the request
            _logger.LogInformation($"Validating that ${nameof(request.StartIndex)} is less than or equal to ${nameof(request.EndIndex)}");

            if (request.StartIndex > request.EndIndex)
            {
                ModelState.AddModelError(nameof(request.StartIndex), "The start index cannot be greater than the end index.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #endregion

            #region Current Memory Usage Check

            if (_memoryUsageMonitor.PrivateMemorySize > request.MaxAllowedMemoryUsage)
            {
                response.Errors.Add($"Current memory usage '{_memoryUsageMonitor.PrivateMemorySize}MB' " +
                                    $"is greater than the specified limit '{request.MaxAllowedMemoryUsage}MB'.");
                return Ok(response);
            }

            #endregion

            #region Background Tasks Creation

            // Create the cancellation token source
            using var source = new CancellationTokenSource();

            // Assign the background tasks that should execute on the thread pool
            _logger.LogInformation("Creating the tasks to execute on the thread pool.");

            var maxMemUsageMonitoringTask = Task.Run(() => _memoryUsageMonitor.MonitorMaxMemUsageAsync(request.MaxAllowedMemoryUsage, source.Token), source.Token);

            var executionTimeoutTask = Task.Delay(request.ExecutionTimeout, source.Token);

            await Task.Delay(1, source.Token); // Small delay before starting the fibonacci task

            var fibonacciGenerationTask = Task.Run(() => _fibonacciGenerator.GenerateSubsequenceAsync(request.StartIndex,
                                                       request.EndIndex,
                                                       request.UseCache,
                                                       source.Token), source.Token);

            #endregion

            #region Asynchronous Wait

            try
            {
                // Wait for any of the tasks to complete
                await Task.WhenAny(executionTimeoutTask, fibonacciGenerationTask, maxMemUsageMonitoringTask);
                // Request for cancellation of all tasks
                source.Cancel();
                // Wait for all tasks to honor the cancellation request
                await Task.WhenAll(executionTimeoutTask, fibonacciGenerationTask, maxMemUsageMonitoringTask);
            }
            catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
            {
                _logger.LogDebug(ex, "TaskCanceled or OperationCanceled exception caught and ignored.");
            } // Ignore task/operation cancellation exceptions
            finally
            {
                _logger.LogDebug($"Tasks statuses: ExecutionTimeoutTask: {executionTimeoutTask.Status}, " +
                                 $"FibonacciGenTask: {fibonacciGenerationTask.Status}, " +
                                 $"MaxMemUsageMonitoringTask: {maxMemUsageMonitoringTask.Status}");
            }

            #endregion

            #region Output Generation

            // Add the errors if any
            if (executionTimeoutTask.IsCompletedSuccessfully)
                response.Errors.Add("Timeout error occurred.");

            if (maxMemUsageMonitoringTask.IsCompletedSuccessfully)
                response.Errors.Add("The maximum memory usage was reached.");

            // Assign the fibonacci subsequence result
            if(fibonacciGenerationTask.IsCompletedSuccessfully)
                response.Result = fibonacciGenerationTask.Result;

            return Ok(response);

            #endregion
        }
    }
}
