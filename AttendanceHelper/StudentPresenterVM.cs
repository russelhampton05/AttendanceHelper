﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttendanceHelper
{
   /// <summary>
   /// Used MVVM to interact with the attendance window. In this file is the VM and the Model.
   /// </summary>
    public class StudentPresenterVM : INotifyPropertyChanged
    {
        #region INotifyImplementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


       
        private ObservableCollection<StudentModel> _tiles;
        public ObservableCollection<StudentModel> Tiles { get { return _tiles; } set { _tiles = value; OnPropertyChanged("Tiles"); } }

        public StudentPresenterVM(List<Student> students)
        {
            Tiles = new ObservableCollection<StudentModel>(populateStudentModel(students));
          
        }

        private List<StudentModel> populateStudentModel(List<Student> students)
        {
            List<StudentModel> returnList = new List<StudentModel>();
            foreach (Student student in students)
            {
                returnList.Add(new StudentModel(student));
            }
            return returnList;
        }
    }

   
    public class StudentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Student student;
        private string _name;
        public string name {get { return _name;} set { _name = value; OnPropertyChanged("name"); } }
        private bool _present;
        public bool present { get { return _present; }set { _present = value;  OnPropertyChanged("present"); } }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ClickCommand { get; private set; }
        
        
        public StudentModel(Student student)
        {
            this.student = student;
            name = student.name;
            present = student.present;
            ClickCommand = new ActionCommand(Click);
           
        }

        private void Click()
        {
            student.present = !student.present;
            present = !present;
        }

    }

   

    public class ActionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action();
        }
    }
}

