namespace Logger;

public abstract class BaseLogger
{
    
    //Auto prop. for class name
    public string ClassName { get; set; } = string.Empty;

    public abstract void Log(LogLevel logLevel, string message);
}

