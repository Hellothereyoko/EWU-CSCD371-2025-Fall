using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Logger;

namespace Logger.Tests
{
    [TestClass]
    public class LogFactoryTests
    {
        private LogFactory _logFactory = null!;
        private const string TestClassName = nameof(LogFactoryTests);

        [TestInitialize]
        public void Setup()
        {
            // Initialize a new LogFactory instance before each test
            _logFactory = new LogFactory();
        }

        [TestMethod]
        public void CreateLogger_ReturnsNull_WhenFileLoggerIsNotConfigured()
        {
            // Arrange
            // LogFactory is initialized but ConfigureFileLogger has NOT been called.

            // Act
            var logger = _logFactory.CreateLogger(TestClassName);

            // Assert
            Assert.IsNull(logger, "Error! CreateLogger must return null if ConfigureFileLogger was not called.");
        }

        [TestMethod]
        public void CreateLogger_ReturnsFileLogger_WhenConfigured()
        {
            // Arrange
            // Setup the logger with a temp file path.
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            _logFactory.ConfigureFileLogger(tempFilePath);

            // Act
            var logger = _logFactory.CreateLogger(TestClassName);

            // Assert
            // Check that the FIleLogger instance was returned.
            Assert.IsNotNull(logger, "Error! CreateLogger must return a logger instance after configuration.");
            // Check that the returned instance is actually a FileLogger.
            Assert.IsTrue(logger is FileLogger, "Error! The returned logger instance must be of type FileLogger.");

            // Cleanup
            File.Delete(tempFilePath);
        }

        [TestMethod]
        public void CreateLogger_PassesClassNameToFileLogger_WhenConfigured()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            _logFactory.ConfigureFileLogger(tempFilePath);
            const string expectedClassName = "TestLoggerClassName";

            // Act
            var logger = _logFactory.CreateLogger(expectedClassName) as FileLogger;

            // Assert
            Assert.IsNotNull(logger, "Error! FileLogger should not be null.");
            Assert.AreEqual(expectedClassName, logger.ClassName, "Error! The ClassName of the FileLogger must match the name associated with CreateLogger.");

            // Delete the temp file
            File.Delete(tempFilePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureFileLogger_ThrowsException_ForNullPath()
        {
            // Arrange & Act
            _logFactory.ConfigureFileLogger(null!);

            // Assert (Handled by attribute)
        }
    }
}
