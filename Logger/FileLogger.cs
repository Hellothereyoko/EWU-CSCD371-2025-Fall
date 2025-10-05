using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Logger
{
    public class FileLogger : BaseLogger
    {
        public string FilePath;

        public FileLogger(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            FilePath = path;
        }

        override
        public void Log(LogLevel logLevel, string message)
        {
            string timeStamp = DateTime.Now.ToString("G"); //COME BACK TO THIS LINE WITH A BETTER UNDERSTANDING OF ClassName
            string logText = $"{timeStamp} {ClassName} {logLevel}: {message}";
            File.AppendAllText(FilePath, logText + Environment.NewLine);
        }
    }
}
