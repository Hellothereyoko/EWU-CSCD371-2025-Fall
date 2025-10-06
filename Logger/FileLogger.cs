using System;
using System.IO;

namespace Logger;

public class FileLogger : BaseLogger
{
    //private store file path 
    private readonly string? _filePath;

    //Constructor to initialize file path
    public FileLogger(string? filePath)
    {
        _filePath = filePath;
    }

    //Override Log method to write log messages to a file
    public override void Log(LogLevel logLevel, string message)
    {
        if (string.IsNullOrEmpty(_filePath))
        {
            throw new InvalidOperationException("File path is not configured.");
        }

        var logEntry = $"{DateTime.Now} [{logLevel}] {ClassName}: {message}{Environment.NewLine}";

        //Append log entry to the specified file
        File.AppendAllText(_filePath, logEntry);
    }
}