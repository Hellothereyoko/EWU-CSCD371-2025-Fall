using System;

namespace Logger;

public static class BaseLoggerMixins
{
    // inside of BaseLoggerMixins create extension methods for:
    // [x] Error
    // [x] Warning
    // [x] Information
    // [x] Debug
    // [x] each of these methods should take in a string for the message and a paramameter array of arguments for the message.
    // [x] Each of these extension methods is expected to be a shortcut for calling the BaseLogger.Log method,
    //     by automatically supplying the appropriate LogLevel
    // [x] These methods should throw an exception if the BaseLogger parameter is null
    //     ensure logger handles this
    public static void Error(this BaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Error, string.Format(message, args));
    }

    public static void Warning(this BaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Warning, string.Format(message, args));
    }

    public static void Information(this BaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Information, string.Format(message, args));
    }

    public static void Debug(this BaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Debug, string.Format(message, args));
    }

    public static BaseLogger EnsureLogger(BaseLogger? logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger;
    }
}
