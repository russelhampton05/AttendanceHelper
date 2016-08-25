using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
namespace AttendanceHelper
{
    /// <summary>
    /// I heavily borrowed the xaml from
    /// http://stackoverflow.com/questions/15652597/wpf-toolkit-for-tile-listview
    /// and adapted it to my needs. Origional post by adabyron. Great example!
    /// 
    /// </summary>
    public partial class AttendanceWindow : Window
    {
        StudentPresenterVM studentVM;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        private bool canClose = true;
        
        public AttendanceWindow(List<Student> students)
        {
            this.KeyDown += new KeyEventHandler(KeyPressed);

            InitializeComponent();
            
            this.WindowStyle = WindowStyle.ToolWindow;
            studentVM = new StudentPresenterVM(students);
            this.DataContext = studentVM;

        }
      
        private void KeyPressed( object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                //do stuff
                canClose = false;
                this.Close();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = canClose;
            base.OnClosing(e);
           
        }

    }
}
