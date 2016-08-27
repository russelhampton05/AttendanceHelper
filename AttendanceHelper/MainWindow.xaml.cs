using System;
using System.Collections.Generic;
using System.ComponentModel;
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
 
    public partial class MainWindow : Window
    {
       

        Logger log = new Logger(Logger.LogLevel.Errors);


        MainVM mainVM;

        public MainWindow()
        {
            InitializeComponent();
            mainVM = new MainVM(log);
            this.DataContext = mainVM;
            
        
        }

       
    }
}
