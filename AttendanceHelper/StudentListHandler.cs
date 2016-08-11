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
        User user = null;
        public StudentListHandler(User user, string baseAddress, Logger log = null)
        { 
            client = new HTTPHandler(baseAddress);
            this.user = user;
            Init(log);
        }
        public StudentListHandler(User user, HTTPHandler client, Logger log = null)
        {
        
            this.client = client;
            this.user = user;
            Init(log);
           
        }
        public void GetStudentList()
        {

        }
        public void Init(Logger log)
        {
        
           if(log != null)
            {
                this.log = log;
            }
           else
            {
                this.log = new Logger(Logger.LogLevel.None);
            }

            LoginHandler login = new LoginHandler(Properties.Resources.LoginEndpoint, user.password, user.username, client, log);
            SettingSelectHandler settings = new SettingSelectHandler(client, Properties.Resources.SetSessionEndpoint, log);

           // settings = new SettingSelectHandler(client, PostGet_WPF.Properties.Resources.SettingEnvironmentEndpoint);
        }
    }

}
