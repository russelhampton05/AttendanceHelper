using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace AttendanceHelper
{
   
    class StudentListHandler
    {
        enum RedirectUri { Login, Settings, AttendanceGet };
        Queue<Tuple<string, RedirectUri>> script;
        Dictionary<string, RedirectUri> endPoints;

        List<Student> students;
        LoginHandler login;
        SettingSelectHandler settings;
        AttendanceHandler attendance;
        const int maxRedirects = 15;
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
        public async void GetStudentList()
        {
            
            int redirects = 0;
            HttpResponseMessage response = null;
            while (redirects < maxRedirects)
            {
                try
                {
                    switch (script.Dequeue().Item2)
                    {
                        case RedirectUri.Login:
                            response = await login.Login(); break;

                        case RedirectUri.Settings:
                            response = await settings.SettingsPost(); break;
                        case RedirectUri.AttendanceGet:
                            response = await attendance.PopulateStudentList(); break;
                        default:
                            break;

                        
                    }
                    enqueueRedirect(script, response);
                }
                catch(LoginPostFailed e)
                {
                    //summon UI or something , user needs interaction
                }
                catch(SetSessionPostFailed e)
                {
                    //Summon UI or something, user needs interaction
                }
                catch(PostFailed e)
                {
                    //Something way bad happened
                }
                catch(GetFailed e)
                {
                    //I should really log
                }
                catch(Exception e)
                {
                    //I'm an awful person.
                }
                 redirects++;
            }

         
            
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

            login = new LoginHandler(Properties.Resources.LoginEndpoint, user, client, log);
            settings = new SettingSelectHandler(client, Properties.Resources.SetSessionEndpoint, log);
            attendance = new AttendanceHandler(Properties.Resources.AttendanceEndpoint, user, client, log);
            endPoints = new Dictionary<string, RedirectUri>();
            populateEndpoints(endPoints);
            script = new Queue<Tuple<string, RedirectUri>>();
            initScript(script);
            
           // settings = new SettingSelectHandler(client, PostGet_WPF.Properties.Resources.SettingEnvironmentEndpoint);
        }

        //the endpoints and order needed to get to the goal
        private void initScript(Queue<Tuple<string, RedirectUri>> script)
        {
            script.Enqueue(new Tuple<string, RedirectUri>(Properties.Resources.LoginEndpoint, RedirectUri.Login));
            script.Enqueue(new Tuple<string, RedirectUri>(Properties.Resources.SetSessionEndpoint, RedirectUri.Settings));
            script.Enqueue(new Tuple<string, RedirectUri>(Properties.Resources.AttendanceEndpoint, RedirectUri.AttendanceGet));
        }
        //all known endpoints for website
        private void populateEndpoints(Dictionary<string, RedirectUri> endpoints)
        {
            endPoints.Add(Properties.Resources.LoginEndpoint, RedirectUri.Login);
            endPoints.Add(Properties.Resources.SetSessionEndpoint, RedirectUri.Settings);
            endpoints.Add(Properties.Resources.AttendanceEndpoint, RedirectUri.AttendanceGet);
        }
        private void enqueueRedirect(Queue<Tuple<string, RedirectUri>> script, HttpResponseMessage response)
        {
            foreach(string endpoint in endPoints.Keys)
            {
                if((response.RequestMessage.RequestUri.ToString().IndexOf(endpoint)!=-1))
                {
                    if (script.Peek().Item1 != endpoint)
                    {
                        script.Enqueue(new Tuple<string, RedirectUri>(endpoint, endPoints[endpoint]));
                    }
                    return;
                }
            }
        }
    }

}
