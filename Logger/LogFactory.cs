namespace Logger;

public class LogFactory
{

    //private store file path 
    private string? _filePath;


    //Config Logger 
    public void ConfigureFileLogger(string filePath)
    {
        _filePath = filePath;
    }


    public BaseLogger? CreateLogger(string className)
    {

        //if not configured, return null
        if (string.IsNullOrEmpty(className))
        {
            return null;
        }

        //Create instance of FileLogger otherwise
        return new FileLogger(_filePath) { ClassName = className };
    }
}
