namespace Logger;

public class LogFactory
{
    private string? _filePath;
    public BaseLogger CreateLogger(string className)
    {
        if (_filePath is null)
        {
            return new FileLogger(className);
        }
        return new FileLogger(_filePath);
    }
    public void ConfigureFileLogger(string filePath)
    {
        _filePath = filePath;
    }
}
