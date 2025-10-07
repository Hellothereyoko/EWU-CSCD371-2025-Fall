using System;

namespace Logger;

public class LogFactory
{
    private string? _filePath;
    public BaseLogger? CreateLogger(string className)
    {
        ArgumentNullException.ThrowIfNull(className);
        if (_filePath is null)
        {
            return null;
        }
        return new FileLogger(_filePath)
        {
        ClassName = className
        };
    }
    public void ConfigureFileLogger(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        _filePath = filePath;
    }
}
