using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttendanceHelper
{
    class MainVM : INotifyPropertyChanged
    { 
        public ICommand LoginCommand { get; private set; }
        public ICommand AttendanceCommand { get; private set; }
        public ICommand SaveUserCommand { get; private set; }

        
        private UserModel _user = new UserModel();
        public UserModel user {get{return _user;} set{_user = value;OnPropertyChanged("user");}}
        
     
        
        Logger log = new Logger(Logger.LogLevel.None);
        public MainVM(Logger log = null)
        {
            if (log != null)
            {
                this.log = log;
            }

            LoginCommand = new ActionCommand(GetUser);
            AttendanceCommand = new ActionCommand(LaunchAttendance);
            SaveUserCommand = new ActionCommand(SaveUser);
            user.appname = "Testtttt";
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
    
        private void LaunchAttendance()
        {
            
        }
        private void SaveUser()
        {

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
        public User user { get { return _user; } set { _user = value; UpdateUser(); OnPropertyChanged("user"); } }

        private string _name = string.Empty;
        public string name { get { return _name; } set { _name = value; _user.username = value; OnPropertyChanged("name"); } }
        private string _password = string.Empty;
        public string password { get { return _password; } set { _user.password = value; _password = value; OnPropertyChanged("password"); } }
        private string _email = string.Empty;
        public string email { get { return _email; } set { _user.email = value; _email = value; OnPropertyChanged("email"); } }
        public string appname { get { return _appname; } set { user.appUser = value; _appname = value; OnPropertyChanged("appname"); } }
        private string _appname = string.Empty;
        
        public UserModel()
        {
            user = new User();
        }
        public UserModel(User user)
        {
            this.user = user;
        }
        private void UpdateUser()
        {
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
