namespace Logger;

public class FileLogger : BaseLogger
{
    public string ClassName { get; }
    public string FilePath { get; }

    public FileLogger(string className, string filePath)
    {
        ClassName = className;
        FilePath = filePath;
    }

}