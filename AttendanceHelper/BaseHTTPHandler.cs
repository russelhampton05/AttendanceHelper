using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AttendanceHelper
{
    //All handlers need these objects, so a simple base class is created for ease of use.
    public class BaseHTTPHandler
    {

        public Logger log = new Logger(Logger.LogLevel.None);
        public HTTPHandler client;


        public BaseHTTPHandler(HTTPHandler client, Logger log = null)
        {
            if (log != null)
            {
                this.log = log;
            }

            this.client = client;
        }
    }
}
