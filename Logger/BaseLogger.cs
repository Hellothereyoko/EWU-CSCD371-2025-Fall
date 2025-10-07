namespace Logger;

public interface BaseLogger
{
    //req1 auto prop
    
    public string ClassName{ get; set} = string.Empty;
  
    public abstract void Log(LogLevel logLevel, string message);
}


    public void Log(LogLevel logLevel, string message)
    {
        throw new System.NotImplementedException();
    }

}