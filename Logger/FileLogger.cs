using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    //req2: Create a FileLogger class that derives from BaseLogger.
    public class FileLogger : BaseLogger
    {
        //req2/part2: it should take in a path to a file to write the log message to
        public string FilePath { get; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;
        }

        //req2/part3:
        // a. it should append messages on their own line in the file
        // b. The output should include all of the following:
        //    i. The current date/time
        //    ii. The class name (from the BaseLogger property)
        //    iii. the log level
        //    iv. The message
        public override void Log(LogLevel logLevel, string message)
        {
            // Implementation to log message to a file at FilePath
        }
    }
}
