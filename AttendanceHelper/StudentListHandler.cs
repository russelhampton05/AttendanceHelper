using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AttendanceHelper
{
    /// <summary>
    /// This class owns handlers. There is one handler per actual webpage in the chain of pages that must be navigated in order to submit attendance
    /// as well as generate the studentlist.
    /// 
    /// This class will also update the student list (attendance) to the website.
    /// </summary>
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

        //loop through the known urls that must be navigated in order. If a redirect recommends that another page gets 
        //visited mid-script, then enqueue this redirect to the script.

        /// <summary>
        /// Returns null on fail
        /// </summary>
        /// <returns></returns>
        public async Task<List<Student>> GetStudentList()
        {

            int redirects = 0;
             HttpResponseMessage response = null;
            try
            {
                while (redirects < maxRedirects && script.Count>0)
                {

                     switch (script.Dequeue().Item2)
                    {
                        case RedirectUri.Login:
                            response = await login.Login(); break;

                        case RedirectUri.Settings:
                            response = await settings.SettingsPost(); break;
                        case RedirectUri.AttendanceGet:
                            response = await attendance.PopulateStudentList();
                            students = attendance.GetStudentList();
                            break;
                        default:
                            break;


                    }
                    enqueueRedirect(script, response);
                    redirects++;
                }
                //TODO: make a better ui than these message boxes

               
            }
            catch (LoginPostFailed e)
            {
                MessageBox.Show("Login failed");
                log.Log(Logger.LogLevel.Errors, "Login failed : " + e.Message);
            }
            catch (SetSessionPostFailed e)
            {
                MessageBox.Show("Failed to set session parameters");
                log.Log(Logger.LogLevel.Errors, "Set session failed : " + e.Message);
            }
            catch (PostFailed e)
            {
                MessageBox.Show("Failed to communicate with site");
                log.Log(Logger.LogLevel.Errors, "Post failed : " + e.Message);
            }
            catch (GetFailed e)
            {
                MessageBox.Show("Failed to communicate with site");
                log.Log(Logger.LogLevel.Errors, "Get failed: " + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Well this is embarassing. Guess you're taking attendance manually today");
                log.Log(Logger.LogLevel.Errors, "Undefined exception: ", e.Message);
            }
            return students;

        }

        public void Init(Logger log)
        {

            if (log != null)
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
        //all known endpoints for website so we know what the redirect messages are telling us. 
        private void populateEndpoints(Dictionary<string, RedirectUri> endpoints)
        {
            endPoints.Add(Properties.Resources.LoginEndpoint, RedirectUri.Login);
            endPoints.Add(Properties.Resources.SetSessionEndpoint, RedirectUri.Settings);
            endpoints.Add(Properties.Resources.AttendanceEndpoint, RedirectUri.AttendanceGet);
        }

        //This is where we check to see if a redirect has meaning to us. If it does, we go there next (if we weren't going there next anyway)
        private void enqueueRedirect(Queue<Tuple<string, RedirectUri>> script, HttpResponseMessage response)
          {
            if(script.Count ==0)
            {
                return;
            }
            foreach (string endpoint in endPoints.Keys)
            {
                if ((response.RequestMessage.RequestUri.ToString().IndexOf(endpoint) != -1))
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
