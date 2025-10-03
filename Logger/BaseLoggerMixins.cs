﻿using System;

namespace Logger;

public static class BaseLoggerMixins
{
    public static void Error(this BaseLogger? logger, string message, params object[] args)
    {
        EnsureLogger(logger).Log(LogLevel.Error, string.Format(message, args));
    }

    public static BaseLogger EnsureLogger(BaseLogger? logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger;
    }
}
