namespace Logger;

public interface BaseLogger
{
    
    //Auto prop. for class name
    string ClassName { get; }


    public void Log(LogLevel logLevel, string message)
    {
        throw new System.NotImplementedException();
    }

}