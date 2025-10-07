using System;

namespace Logger;

public class ConsoleLogger : BaseLogger
{
    public string ClassName { get; set; } = string.Empty;

    public void Log(LogLevel logLevel, string message)
    {
        string timestamp = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt");
        string logEntry = $"{timestamp} {ClassName} {logLevel}: {message}";
        
        Console.WriteLine(logEntry);
    }
}