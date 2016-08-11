using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AttendanceHelper
{
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
