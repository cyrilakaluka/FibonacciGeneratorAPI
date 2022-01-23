using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FibonacciGeneratorAPI.Controllers;
using FibonacciGeneratorAPI.DTOs.Requests;
using FibonacciGeneratorAPI.DTOs.Responses;
using FibonacciGeneratorAPI.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FibonacciGeneratorAPI.UnitTests
{
    public class FibonacciSubsequenceControllerTest
    {
        private readonly Mock<IFibonacciGenerator> _fibonacciGeneratorStub = new();
        private readonly Mock<IMemoryUsageMonitor> _memoryUsageMonitorStub = new();
        private readonly Mock<ILogger<FibonacciSubsequenceController>> _loggerStub = new();

        [Fact]
        public async Task GetFibonacciSubsequence_WithStartIndexGreaterThanEndIndex_ReturnsBadRequest()
        {
            // Arrange
            var request = new FibonacciSubsequenceRequest { StartIndex = 10, EndIndex = 2 };
            var controller = new FibonacciSubsequenceController(_fibonacciGeneratorStub.Object, _memoryUsageMonitorStub.Object, _loggerStub.Object);

            // Act
            var output = await controller.GetFibonacciSubsequence(request);
            var result = output as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [MemberData(nameof(FibonacciSubsequenceControllerTestDataSource.ValidStartingAndEndingIndexTestData), MemberType = typeof(FibonacciSubsequenceControllerTestDataSource))]
        public async Task GetFibonacciSubsequence_WithValidStartingAndEndingIndex_ReturnsExpectedResult(int startIndex, int endIndex, string[] expectedResult)
        {
            // Arrange
            var request = new FibonacciSubsequenceRequest { StartIndex = startIndex, EndIndex = endIndex };
            _fibonacciGeneratorStub
                .Setup(f => f.GenerateSubsequenceAsync(startIndex, endIndex, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);
            _memoryUsageMonitorStub.Setup(m => m.MonitorMaxMemUsageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>()))
                                   .Returns(async (int _, CancellationToken c, int _) =>
                                   {
                                       while (true)
                                       {
                                           await Task.Delay(0, c);
                                           if (c.IsCancellationRequested)
                                               break;
                                       }
                                   });
            var controller = new FibonacciSubsequenceController(_fibonacciGeneratorStub.Object, _memoryUsageMonitorStub.Object, _loggerStub.Object);

            // Act
            var output = await controller.GetFibonacciSubsequence(request);
            var result = output as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<FibonacciSubsequenceResponse>().Which.Result.Should()
                  .BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(FibonacciSubsequenceControllerTestDataSource.ExecutionTimeoutTestData), MemberType = typeof(FibonacciSubsequenceControllerTestDataSource))]
        public async Task GetFibonacciSubsequence_WithExecutionTimeout_ReturnsTimeoutError(string[] outputSequence)
        {
            // Arrange
            var request = new FibonacciSubsequenceRequest { StartIndex = 0, EndIndex = 2, ExecutionTimeout = 1000 };
            _fibonacciGeneratorStub.Setup(f => f.GenerateSubsequenceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                                   .Returns(async (int _, int _, bool _, CancellationToken c) =>
                                   {
                                       while (true)
                                       {
                                           await Task.Delay(0);
                                           if (c.IsCancellationRequested)
                                               break;
                                       }
                                       return outputSequence;
                                   });
            _memoryUsageMonitorStub.Setup(m => m.MonitorMaxMemUsageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>()))
                                   .Returns(async (int _, CancellationToken c, int _) =>
                                   {
                                       while (true)
                                       {
                                           await Task.Delay(0, c);
                                           if (c.IsCancellationRequested)
                                               break;
                                       }
                                   });
            var controller = new FibonacciSubsequenceController(_fibonacciGeneratorStub.Object, _memoryUsageMonitorStub.Object, _loggerStub.Object);
            
            // Act
            var output = await controller.GetFibonacciSubsequence(request);
            var result = output as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<FibonacciSubsequenceResponse>().Which.Result.Should().BeEquivalentTo(outputSequence);
            result.Value.Should().BeOfType<FibonacciSubsequenceResponse>().Which.Errors.ElementAt(0).Should().BeEquivalentTo("Timeout error occurred.");
        }

        [Fact]
        public async Task GetFibonacciSubsequence_WithMaxMemoryUsageReached_ReturnsMaxMemoryError()
        {
            // Arrange
            var request = new FibonacciSubsequenceRequest { StartIndex = 0, EndIndex = 2, ExecutionTimeout = 1000 };
            _fibonacciGeneratorStub.Setup(f => f.GenerateSubsequenceAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                                   .Returns(async (int _, int _, bool _, CancellationToken c) =>
                                   {
                                       while (true)
                                       {
                                           await Task.Delay(0);
                                           if (c.IsCancellationRequested)
                                               break;
                                       }
                                       return new[] { "0", "1", "1", "3", "5" };
                                   });
            _memoryUsageMonitorStub.Setup(m => m.MonitorMaxMemUsageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<int>()))
                                   .Returns(async (int _, CancellationToken c, int _) =>
                                   {
                                       await Task.Delay(50, c);
                                   });
            var controller = new FibonacciSubsequenceController(_fibonacciGeneratorStub.Object, _memoryUsageMonitorStub.Object, _loggerStub.Object);

            // Act
            var output = await controller.GetFibonacciSubsequence(request);
            var result = output as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<FibonacciSubsequenceResponse>().Which.Result.Should().BeEquivalentTo(new[] { "0", "1", "1", "3", "5" });
            result.Value.Should().BeOfType<FibonacciSubsequenceResponse>().Which.Errors.ElementAt(0).Should().BeEquivalentTo("The maximum memory usage was reached.");
        }

        private static class FibonacciSubsequenceControllerTestDataSource
        {
            public static IEnumerable<object[]> ValidStartingAndEndingIndexTestData => new List<object[]>
            {
                new object[] { 0, 5, new[] { "0", "1", "1", "3", "5" } },
                new object[] { 4, 10, new[] { "3", "5", "8", "13", "21", "34", "55" } }
            };

            public static IEnumerable<object[]> ExecutionTimeoutTestData => new List<object[]>
            {
                new object[] { System.Array.Empty<string>() },
                new object[] { new[] { "0", "1", "1", "3", "5" } }
            };
        }
    }
}
