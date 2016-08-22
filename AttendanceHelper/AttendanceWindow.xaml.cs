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

namespace AttendanceHelper
{
    /// <summary>
    /// Interaction logic for AttendanceWindow.xaml
    /// </summary>
    public partial class AttendanceWindow : Window
    {
        StudentPresenterVM studentVM;
        public AttendanceWindow(List<Student> students)
        {
            InitializeComponent();
            studentVM = new StudentPresenterVM(students);
            this.DataContext = studentVM;
        }
    }
}
