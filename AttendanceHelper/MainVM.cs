using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AttendanceHelper
{
    class MainVM : INotifyPropertyChanged
    {
        public event EventHandler attendanceLaunched;
        public event EventHandler emailSent;

        public ICommand LoginCommand { get; private set; }
        public ICommand AttendanceCommand { get; private set; }
        public ICommand SaveUserCommand { get; private set; }

        System.Timers.Timer emailTimer;

        List<Student> students = new List<Student>();


        private DateTime _emailTime;
        public DateTime emailTime { get { return _emailTime; } set { _emailTime = value; OnPropertyChanged("emailTime"); } }

        private DateTime _windowCloseTime;
        public DateTime windowCloseTime { get { return _windowCloseTime; } set { _windowCloseTime= value; OnPropertyChanged("windowCloseTime"); } }

        private UserModel _user = new UserModel();
        public UserModel user {get{return _user;} set{_user = value;OnPropertyChanged("user");}}

       

        Logger log = new Logger(Logger.LogLevel.None);
        public MainVM(Logger log = null)
        {
            if (log != null)
            {
                this.log = log;
            }

          
            emailTime = DateTime.Now.AddMinutes(Double.Parse(Properties.Resources.DefaultWaitTime));
            windowCloseTime = DateTime.Now.AddMinutes(Double.Parse(Properties.Resources.DefaultWaitTime));
            LoginCommand = new ActionCommand(GetUser);
            AttendanceCommand = new ActionCommand(LaunchAttendance);
            SaveUserCommand = new ActionCommand(SaveUser);
        }
        private void LaunchTimer()
        {
            var timespan = emailTime.TimeOfDay - DateTime.Now.TimeOfDay;
            if (timespan.TotalMilliseconds > 0)
            {
                emailTimer = new System.Timers.Timer(timespan.TotalMilliseconds);
                emailTimer.AutoReset = false;
                emailTimer.Elapsed += SendEmail;
                emailTimer.Start();
            }
        }
        private void SendEmail(object sender, System.Timers.ElapsedEventArgs args)
        {
         
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(user.email_from);
            msg.To.Add(user.email);
            msg.Subject = Properties.Resources.EmailSubject + " " + DateTime.Now.ToString();
            msg.Body = BuildEmailBody();
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(user.email_from, user.email_password);
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                log.Log(Logger.LogLevel.Errors, "Email failed with error: " + ex.Message);
            }
            finally
            {
                msg.Dispose();
            }

            EventHandler handler = emailSent;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
       private string BuildEmailBody()
        {
            string studentList = string.Empty;
            studentList += "The following students were recorded as not here: " + Environment.NewLine;
            foreach(Student student in students)
            {
                if(!student.present)
                {
                    studentList += student.name + Environment.NewLine;
                }
            }

            return studentList;
        }


    
        private void GetUser()
        {
            if(String.IsNullOrEmpty(user.appname))
            {
                return;
            }
            UserManager um = new UserManager(log);

            var loggedInUser = um.GetUser(user.appname);
            if(loggedInUser!=null)
            {
                user.user = loggedInUser;
            }
            else
            {
                System.Windows.MessageBox.Show("User not found, try saving a new user"); 
            }
        }
    
        private async void LaunchAttendance()
        {
            if (!user.CheckUserFilled())
            {
                return;
            }

            EventHandler handler = attendanceLaunched;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
            LaunchTimer();
            students = new List<Student>();
          
            StudentListHandler studentHandler = new StudentListHandler(user.user,Properties.Resources.BaseUrl, log);
          
            students = await studentHandler.GetStudentList();
            if(students == null)
            {
                return;
            }
            AttendanceWindow attendanceWindow;
            attendanceWindow = new AttendanceWindow(students, windowCloseTime);
            attendanceWindow.ShowDialog();

        }
       
        private void SaveUser()
        {
            if(user.user ==null)
            {
                return;
            }
            if(!user.CheckUserFilled())
            {
                MessageBox.Show("Please fill out app name, website name, and website password to store a new user");
                return;
            }
            UserManager um = new UserManager(log);
            user.UpdateUser();
            if(!um.SaveUser(user.user))
            {
                MessageBox.Show("Saved user failed");
            }
        }

        #region Yayboilerplatecode
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
      

    }

    public class UserModel : INotifyPropertyChanged
    {
        private User _user;
        public User user { get { return _user; } set { _user = value; UpateUserModel(); OnPropertyChanged("user"); } }

        private string _name = string.Empty;
        public string name { get { return _name; } set { _name = value; _user.username = value; OnPropertyChanged("name"); } }
        private string _password = string.Empty;
        public string password { get { return _password; } set { _user.password = value; _password = value; OnPropertyChanged("password"); } }
        private string _email = string.Empty;
        public string email { get { return _email; } set { _user.email = value; _email = value; OnPropertyChanged("email"); } }
        private string _email_from = string.Empty;
        public string email_from { get { return _email_from; } set { _user.email_from = value; _email_from = value; OnPropertyChanged("email_from"); } }
        private string _email_password = string.Empty;
        public string email_password { get { return _email_password; } set { _user.email_password = value; _email_password = value; OnPropertyChanged("email_password"); } }
        public string appname { get { return _appname; } set { user.appUser = value; _appname = value; OnPropertyChanged("appname"); } }
        private string _appname = string.Empty;
        
    

        public bool CheckUserFilled()
        {
            bool isFilled = true;
            if(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(appname) || String.IsNullOrEmpty(password))
            {
                isFilled = false;
            }
            return isFilled;
        }
        public UserModel()
        {
            user = new User();
        }
        public UserModel(User user)
        {
            this.user = user;
        }
        public void UpdateUser(User user)
        {
            this.user = new User(user);
        }
        public void UpdateUser()
        {
            user.appUser = appname;
            user.username = name;
            user.password = password;
            user.email = email;
            user.email_from = email_from;
            user.email_password = email_password;
         
        }
        private void UpateUserModel()
        {
            email_password = user.email_password;
            email_from = user.email_from;
            name = user.username;
            appname = user.appUser;
            password = user.password;
            email = user.email;
        }

        #region Yayboilerplatecode
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
