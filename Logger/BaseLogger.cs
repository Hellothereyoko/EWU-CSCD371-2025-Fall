namespace Logger;

public interface BaseLogger
{

    //Auto prop. for class name
    string ClassName { get; set; }


    void Log(LogLevel logLevel, string message);

}