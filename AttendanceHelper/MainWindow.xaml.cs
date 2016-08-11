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
        string _postContent;
        string _extensionUrl;
        string _baseUrl;
        string responseText;
        Logger log = new Logger(Logger.LogLevel.Verbose);
        HTTPHandler client = null;
        LoginHandler login = null;
        public void UpdateStrings()
        {
            _postContent = postContent.Text;
            _extensionUrl = extensionURL.Text;
            _baseUrl = baseUri.Text;
            responseText = ResponseBox.Text;
        }
        public void UpdateUI()
        {
            postContent.Text = _postContent;
            extensionURL.Text = _extensionUrl;
            baseUri.Text = _baseUrl;
            ResponseBox.Text = responseText;
        }
        public MainWindow()
        {
            InitializeComponent();
            baseUri.Text = @"https://grades.tomballisd.net/TAC/";

        }

        private async void MakePost_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            if (client == null)
                return;

            var response = await client.MakePost(new StringContent(_postContent), _extensionUrl);
            responseText = await GetResponseString(response);
            UpdateUI();
        }

        private async void MakeGet_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            if (client == null)
                return;

            var resopnse = await client.MakeGet(_extensionUrl);
            responseText = await GetResponseString(resopnse);
            UpdateUI();
        }
        private async Task<string> GetResponseString(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
        private void MakeClient_Click(object sender, RoutedEventArgs e)
        {
            UpdateStrings();
            client = new HTTPHandler(_baseUrl, log);
            login = new LoginHandler(string.Empty, string.Empty, string.Empty, client);
        }

        private async void LogOn_Click(object sender, RoutedEventArgs e)
        {
            var response = await login.Login();
            responseText = await response.Content.ReadAsStringAsync();
            UpdateUI();

        }
    }
}
