﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Logger.Tests;

[TestClass]
public class BaseLoggerMixinsTests
{
    [TestMethod]
    public void Error_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? logger = null;
        string message = "";

        // Act
        void Act() => BaseLoggerMixins.Error(logger, message);

        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(Act);
    }

    [TestMethod]
    public void EnsureLogger_WithNullLogger_ThrowsException()
    {
        // Arrange
        BaseLogger? logger = null;
        // Act
        void Act() => BaseLoggerMixins.EnsureLogger(logger);
        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(Act);
    }

    [TestMethod]
    public void Error_WithData_LogsMessage()
    {
        // Arrange
        var logger = new TestLogger();

        // Act
        logger.Error("Message {0}", 42);

        // Assert
        Assert.AreEqual(1, logger.LoggedMessages.Count);
        Assert.AreEqual(LogLevel.Error, logger.LoggedMessages[0].LogLevel);
        Assert.AreEqual("Message 42", logger.LoggedMessages[0].Message);
    }

}

public class TestLogger : BaseLogger
{
    public List<(LogLevel LogLevel, string Message)> LoggedMessages { get; } = new List<(LogLevel, string)>();

    public override void Log(LogLevel logLevel, string message)
    {
        LoggedMessages.Add((logLevel, message));
    }
}
