using System;

namespace Logger;

public class LogFactory
{
    private string? _filePath;
    public BaseLogger? CreateLogger(string className)
    {
        if (className is null)
        {
            throw new ArgumentNullException("className");
        }
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
        if (filePath is null)
        {
            throw new ArgumentNullException("filePath");
        }
        _filePath = filePath;
    }
}
