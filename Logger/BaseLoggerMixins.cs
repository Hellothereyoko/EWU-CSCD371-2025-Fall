using System;

namespace Logger;

public static class BaseLoggerMixins
{
    public static void Error(this IBaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Error, string.Format(message, args));
    }

    public static void Warning(this IBaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Warning, string.Format(message, args));
    }

    public static void Information(this IBaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Information, string.Format(message, args));
    }

    public static void Debug(this IBaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Debug, string.Format(message, args));
    }

    public static IBaseLogger EnsureLogger(IBaseLogger? logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger;
    }
}
