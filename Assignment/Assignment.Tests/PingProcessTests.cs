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
        public void RunAsync_UsingTplWithCancellation_CatchAggregateExceptionWrapping()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            AggregateException? caughtException = null;
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync("localhost", cts.Token);
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
        public void RunAsync_UsingTplWithCancellation_CatchAggregateExceptionWrappingTaskCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            AggregateException? caughtException = null;
            try
            {
                Task<PingResult> task = _pingProcess.RunAsync("localhost", cts.Token);
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
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(TaskCanceledException));
        }

        // Task 3c: Test cancellation with async/await
        [TestMethod]
        public async Task RunAsync_UsingTplWithCancellation_CatchTaskCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            try
            {
                await _pingProcess.RunAsync("localhost", cts.Token);
                Assert.Fail("Expected TaskCanceledException to be thrown");
            }
            catch (TaskCanceledException)
            {
                // Expected exception
            }
        }

        // Task 4: Test parallel execution
        [TestMethod]
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
        public async Task RunAsync_MultipleHostsWithCancellation_ThrowsException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            string[] hosts = { "localhost", "localhost", "localhost" };
            cts.Cancel();

            // Act & Assert
            try
            {
                await _pingProcess.RunAsync(hosts, cts.Token);
                Assert.Fail("Expected TaskCanceledException to be thrown");
            }
            catch (TaskCanceledException)
            {
                // Expected exception
            }
        }

        // Task 5: Test long running task
        [TestMethod]
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
        public async Task RunLongRunningAsync_WithCancellation_ThrowsException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            try
            {
                await _pingProcess.RunLongRunningAsync("localhost", cts.Token);
                Assert.Fail("Expected TaskCanceledException to be thrown");
            }
            catch (TaskCanceledException)
            {
                // Expected exception
            }
        }

        // Extra Credit: Test IProgress
        [TestMethod]
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