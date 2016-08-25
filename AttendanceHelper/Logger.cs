using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AttendanceHelper
{
    /// <summary>
    /// Simple logging class. The Loglevel enum is used instead of a nullobject pattern. 
    /// Objects that aren't sure if they should log or not make their own logger with log level None. 
    /// If their callers want to change this, they use dependency injection to create a new logger with a
    /// log level that is not none.
    /// </summary>
    public class Logger
    {
        public enum LogLevel { None, Errors, Verbose };
        string writeDirectory =Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/" + System.AppDomain.CurrentDomain.FriendlyName + "_Log.txt";
        LogLevel logLevel;
        string openingMessage = "Log start time: " + Environment.NewLine + "-----------------------";
        public Logger(LogLevel logLevel = LogLevel.None, string writeDirectory = "")
        {
            if(writeDirectory != String.Empty)
                this.writeDirectory = writeDirectory;

            this.logLevel = logLevel;
            File.AppendAllText(this.writeDirectory, openingMessage + DateTime.Now.ToString() + Environment.NewLine);
        }
        public virtual void Log(LogLevel level, params string[] args)
        {
            if (logLevel >= level)
            {
                File.AppendAllText(writeDirectory, "----------------------" + Environment.NewLine);
                File.AppendAllLines(writeDirectory, args);
            }
        }
        public void ClearLog()
        {
            File.WriteAllText(writeDirectory, string.Empty);
        }

    }
}
