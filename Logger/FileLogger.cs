using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;


namespace Logger
{
    public class FileLogger : BaseLogger
    {
        private readonly string _filePath;

        public FileLogger(string path)
        {
            ArgumentNullException.ThrowIfNull("path");
            _filePath = path;
        }

        override
        public void Log(LogLevel logLevel, string message)
        {
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture); //COME BACK TO THIS LINE WITH A BETTER UNDERSTANDING OF ClassName
            string logText = $"{timeStamp} {ClassName} {logLevel}: {message}";
            File.AppendAllText(_filePath, logText + Environment.NewLine);
        }
    }
}
