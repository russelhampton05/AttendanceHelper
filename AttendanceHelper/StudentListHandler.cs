using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AttendanceHelper
{
   
    class StudentListHandler
    {
        List<Student> students;
        
        LoginHandler login;
        SettingSelectHandler settings;
        HTTPHandler client = null;
        Logger log = null;
        public StudentListHandler(string baseAddress, Logger log = null)
        {
            client = new HTTPHandler(baseAddress);
            Init(log);
        }
        public StudentListHandler(HTTPHandler client, Logger log = null)
        {
            this.client = client;
            
            Init(log);
           
        }
        public void GetStudentList()
        {

        }
        public void Init(Logger log)
        {
            //find an android friendly way to do this without using resource
           if(log != null)
            {
                this.log = log;
            }
           else
            {
                this.log = new Logger(Logger.LogLevel.None);
            }
           //login = new LoginHandler(,
           //    PostGet_WPF.Properties.Resources.Password,
           //    PostGet_WPF.Properties.Resources.UserName,
           //    client);

           // settings = new SettingSelectHandler(client, PostGet_WPF.Properties.Resources.SettingEnvironmentEndpoint);
        }
    }

}
