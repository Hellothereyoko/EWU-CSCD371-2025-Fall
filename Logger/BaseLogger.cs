namespace Logger;

public abstract class BaseLogger
{
    //req1 auto prop
    
    public string ClassName{ get; set} = string.Empty;
    public abstract void Log(LogLevel logLevel, string message);
}

