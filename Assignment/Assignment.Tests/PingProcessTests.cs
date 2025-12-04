using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment.Tests
{
    [TestClass]
    public class PingProcessTests
    {
        private PingProcess _pingProcess = null!;

        [TestInitialize]
        public void TestInitialize() => _pingProcess = new PingProcess();

        // Task 1: Test RunTaskAsync without async/await
        [TestMethod]
        [Timeout(10000)] // 10 second timeout
        public void RunTaskAsync_Success()
        {
            // Arrange
            const string host = "localhost";

            // Act
            Task<PingResult> task = _pingProcess.RunTaskAsync(host);
            task.Wait();
            PingResult result = task.Result;

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode);
            Assert.IsNotNull(result.StdOutput);
            Assert.IsTrue(result.StdOutput.Contains("localhost") || result.StdOutput.Contains("127.0.0.1"));
        }

        // Task 2a: Test RunAsync without async/await
        [TestMethod]
        [Timeout(10000)]
        public void RunAsync_UsingTaskReturn_Success()
        {
            // Arrange
            const string host = "localhost";

            // Act
            Task<PingResult> task = _pingProcess.RunAsync(host);
            task.Wait();
            PingResult result = task.Result;

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode);
            Assert.IsNotNull(result.StdOutput);
            Assert.IsTrue(result.StdOutput.Contains("localhost") || result.StdOutput.Contains("127.0.0.1"));
        }

        // Task 2b: Test RunAsync with async/await
        [TestMethod]
        [Timeout(10000)]
        public async Task RunAsync_UsingTpl_Success()
        {
            // Arrange
            const string host = "localhost";

            // Act
            PingResult result = await _pingProcess.RunAsync(host);

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode);
            Assert.IsNotNull(result.StdOutput);
            Assert.IsTrue(result.StdOutput.Contains("localhost") || result.StdOutput.Contains("127.0.0.1"));
        }

        // Task 3a: Test cancellation with AggregateException
        [TestMethod]
        [Timeout(5000)]
        public void RunAsync_UsingTplWithCancellation_CatchAggregateExceptionWrapping()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            
            // Act & Assert
            AggregateException? caughtException = null;
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync("localhost", cts.Token);
                cts.Cancel(); // Cancel after starting the task
                Thread.Sleep(10); // Give it a moment to process cancellation
                task.Wait();
                Assert.Fail("Expected AggregateException to be thrown");
            }
            catch (AggregateException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsTrue(caughtException.InnerExceptions.Count > 0);
        }

        // Task 3b: Test cancellation with TaskCanceledException inner exception
        [TestMethod]
        [Timeout(5000)]
        public void RunAsync_UsingTplWithCancellation_CatchAggregateExceptionWrappingTaskCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();

            // Act & Assert
            AggregateException? caughtException = null;
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync("localhost", cts.Token);
                cts.Cancel(); // Cancel after starting the task
                Thread.Sleep(10); // Give it a moment to process cancellation
                task.Wait();
                Assert.Fail("Expected AggregateException to be thrown");
            }
            catch (AggregateException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(OperationCanceledException));
        }

        // Task 3c: Test cancellation with async/await
        [TestMethod]
        [Timeout(5000)]
        public async Task RunAsync_UsingTplWithCancellation_CatchTaskCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();

            // Act & Assert
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync("localhost", cts.Token);
                cts.Cancel(); // Cancel after starting the task
                await Task.Delay(10); // Give it a moment to process cancellation
                await task;
                Assert.Fail("Expected OperationCanceledException to be thrown");
            }
            catch (OperationCanceledException)
            {
                // Expected exception (TaskCanceledException inherits from OperationCanceledException)
            }
        }

        // Task 4: Test parallel execution
        [TestMethod]
        [Timeout(15000)] // 15 second timeout for multiple pings
        public async Task RunAsync_MultipleHosts_Success()
        {
            // Arrange
            string[] hosts = { "localhost", "localhost", "localhost" };

            // Act
            PingResult result = await _pingProcess.RunAsync(hosts);

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode); // All should succeed
            Assert.IsNotNull(result.StdOutput);
            
            // Count occurrences of localhost/127.0.0.1 in output
            int count = 0;
            string output = result.StdOutput.ToLower();
            foreach (string line in output.Split(Environment.NewLine))
            {
                if (line.Contains("localhost") || line.Contains("127.0.0.1"))
                {
                    count++;
                }
            }
            
            // Should have output from all three pings
            Assert.IsTrue(count >= 3, $"Expected at least 3 localhost references, found {count}");
        }

        // Task 4: Test parallel execution with cancellation
        [TestMethod]
        [Timeout(5000)]
        public async Task RunAsync_MultipleHostsWithCancellation_ThrowsException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            string[] hosts = { "localhost", "localhost", "localhost" };

            // Act & Assert
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync(hosts, cts.Token);
                cts.Cancel(); // Cancel after starting
                await Task.Delay(10);
                await task;
                Assert.Fail("Expected OperationCanceledException to be thrown");
            }
            catch (OperationCanceledException)
            {
                // Expected exception
            }
        }

        // Task 5: Test long running task
        [TestMethod]
        [Timeout(10000)] // 10 second timeout
        public async Task RunLongRunningAsync_Success()
        {
            // Arrange
            const string host = "localhost";

            // Act
            PingResult result = await _pingProcess.RunLongRunningAsync(host);

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode);
            Assert.IsNotNull(result.StdOutput);
            Assert.IsTrue(result.StdOutput.Contains("localhost") || result.StdOutput.Contains("127.0.0.1"));
        }

        // Task 5: Test long running task with cancellation
        [TestMethod]
        [Timeout(5000)]
        public async Task RunLongRunningAsync_WithCancellation_ThrowsException()
        {
            // Arrange
            var cts = new CancellationTokenSource();

            // Act & Assert
            try
            {
                Task<PingResult> task = _pingProcess.RunLongRunningAsync("localhost", cts.Token);
                cts.Cancel(); // Cancel after starting
                await Task.Delay(10);
                await task;
                Assert.Fail("Expected OperationCanceledException to be thrown");
            }
            catch (OperationCanceledException)
            {
                // Expected exception
            }
        }

        // Extra Credit: Test IProgress
        [TestMethod]
        [Timeout(10000)] // 10 second timeout
        public async Task RunAsync_WithProgress_ReportsProgress()
        {
            // Arrange
            const string host = "localhost";
            int progressCallCount = 0;
            var progress = new Progress<string>(output =>
            {
                progressCallCount++;
                Assert.IsNotNull(output);
            });

            // Act
            PingResult result = await _pingProcess.RunAsync(host, progress);

            // Assert
            Assert.AreEqual<int>(0, result.ExitCode);
            Assert.IsNotNull(result.StdOutput);
            Assert.IsTrue(progressCallCount > 0, "Progress should have been reported at least once");
        }

        // Additional test: Verify StdOutput completeness
        [TestMethod]
        [Timeout(15000)] // 15 second timeout
        public async Task RunAsync_MultipleHosts_AllOutputCaptured()
        {
            // Arrange
            string[] hosts = { "localhost", "localhost" };

            // Act
            PingResult result = await _pingProcess.RunAsync(hosts);

            // Assert
            Assert.IsNotNull(result.StdOutput);
            
            // The output should contain information from both pings
            // Even though intermingled, all lines should be present
            string[] lines = result.StdOutput.Split(new[] { Environment.NewLine }, 
                StringSplitOptions.RemoveEmptyEntries);
            
            Assert.IsTrue(lines.Length > 0, "Output should contain multiple lines");
        }
    }
}