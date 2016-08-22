using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttendanceHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //data binding is for nubs
      
        string _username;
        string _password;
        string _key;
      
        string _appUser;
     
        Logger log = new Logger(Logger.LogLevel.Errors);
        HTTPHandler client = null;
        LoginHandler login = null;
        UserManager usermananger;
        User user;
        StudentListHandler studentListHandler;

        AttendanceWindow attendanceWindow;
        public void UpdateStrings()
        {
        
            _username = userName.Text;
            _password = password.Text;
            _appUser = appUser.Text;
            _key = key.Text;
           
        }
        public void UpdateUI()
        {
          
            userName.Text = _username;
            password.Text = _password;
            appUser.Text = _appUser;
            key.Text = _key;
        }
        public MainWindow()
        {
            InitializeComponent();
            usermananger = new UserManager(log);
        
        }

        private async Task<string> GetResponseString(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
        private void MakeClient_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            client = new HTTPHandler(Properties.Resources.BsaeUrl, log);
            login = new LoginHandler(Properties.Resources.LoginEndpoint, user, client);
        }

        private void makeUser_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            User user = new User();
            user.appUser = _appUser;
            user.password = password.Text;
            user.username = userName.Text;
            _key = string.Empty;
            foreach(byte b in user.key) { _key += b.ToString(); }
            usermananger.SaveUser(user);
            UpdateUI();
        }

        private void getUser_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            User user = usermananger.GetUser(_appUser);
            if(user!=null)
            {
                _username = user.username;
                _password = user.password;
                _key = string.Empty;
                foreach (byte b in user.key) { _key += b.ToString(); }
                this.user = user;
            }
            UpdateUI();
        }

        private void getStudents_Click(object sender, RoutedEventArgs e)
        {
            MakeClient_Click(new object(), new RoutedEventArgs());
            user = usermananger.GetUser("jessica");
            studentListHandler = new StudentListHandler(user, client, log);
            var students = studentListHandler.GetStudentList();
            
        }

        private async void launchAttendance_Click(object sender, RoutedEventArgs e)
        {
            MakeClient_Click(new object(), new RoutedEventArgs());
            user = usermananger.GetUser("jessica");
            studentListHandler = new StudentListHandler(user, client, log);
            var students = await studentListHandler.GetStudentList();
            attendanceWindow = new AttendanceWindow(students);
            this.Hide();
            attendanceWindow.ShowDialog();
        }
    }
}
