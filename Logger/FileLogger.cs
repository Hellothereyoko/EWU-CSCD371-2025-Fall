using System;
using System.IO;

namespace Logger;

public class FileLogger : BaseLogger
{
    public string ClassName { get; set; } = string.Empty;
    public string FilePath { get; }

    public FileLogger(string filePath)
    {
        FilePath = filePath;
    }

    public void Log(LogLevel logLevel, string message)
    {
        string timestamp = DateTime.Now.ToString("M/d/yyyy h:mm:ss tt");
        string logEntry = $"{timestamp} {ClassName} {logLevel}: {message}";
        
        File.AppendAllText(FilePath, logEntry + Environment.NewLine);
    }
}