using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Logger;

namespace Logger.Tests;

[TestClass]
public class BaseLoggerMixinsTests
{
    [TestMethod]
    public void Error_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? nullLogger = null;
        // Act
        Assert.ThrowsExactly<ArgumentNullException>(() => BaseLoggerMixins.Error(nullLogger!, "Test Error message."));

        // Assert
        // Empty unless try catch is used.
    }

    [TestMethod]
    public void Error_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();

        // Act
        logger.Error("Test: Critical failure in module {0}.", "TestModule");

        // Assert
        Assert.AreEqual(1, logger.LoggedMessages.Count);
        Assert.AreEqual(LogLevel.Error, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Test: Critical failure in module TestModule.", logger.LoggedMessages[0].Message);
    }

    // Test Warnings
    [TestMethod]
    public void Warning_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? nullLogger = null;
        // Act
        Assert.ThrowsExactly<ArgumentNullException>(() => BaseLoggerMixins.Warning(nullLogger!, "Test Warning message."));
        // Assert
    }

    [TestMethod]
    public void Warning_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();
        // Act
        logger.Warning("Test: Config value {0} is outside recommended range.", "TestThreads");
        // Assert
        Assert.AreEqual(1, logger.LoggedMessages.Count);
        Assert.AreEqual(LogLevel.Warning, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Test: Config value TestThreads is outside recommended range.", logger.LoggedMessages[0].Message);
    }

    // Test Information
    [TestMethod]
    public void Information_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? nullLogger = null;
        // Act
        Assert.ThrowsExactly<ArgumentNullException>(() => BaseLoggerMixins.Information(nullLogger!, "Test Information message."));
        // Assert
    }

    [TestMethod]
    public void Information_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();
        // Act
        logger.Information("Test: Application startup in {0}ms.", 2300);
        // Assert
        Assert.AreEqual(1, logger.LoggedMessages.Count);
        Assert.AreEqual(LogLevel.Information, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Test: Application startup in 2300ms.", logger.LoggedMessages[0].Message);
    }

    // Test Debug
    [TestMethod]
    public void Debug_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? nullLogger = null;
        // Act
        Assert.ThrowsExactly<ArgumentNullException>(() => BaseLoggerMixins.Debug(nullLogger!, "Test Debug message."));
        // Assert
    }

    [TestMethod]
    public void Debug_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();
        // Act
        logger.Debug("Test: Processed array length: {0}. Checksum: {1:X}.", 22, 0xDEADBEEF);
        // Assert
        Assert.AreEqual(1, logger.LoggedMessages.Count);
        Assert.AreEqual(LogLevel.Debug, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Test: Processed array length: 22. Checksum: DEADBEEF.", logger.LoggedMessages[0].Message);
    }

}

public class TestLogger : BaseLogger
{
    //public override string ClassName { get; set; } = nameof(TestLogger);
    public List<(LogLevel LogLevel, string Message)> LoggedMessages { get; } = new List<(LogLevel, string)>();

    public override void Log(LogLevel logLevel, string message)
    {
        LoggedMessages.Add((logLevel, message));
    }
}

