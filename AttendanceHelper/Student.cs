using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceHelper
{
    public class Student
    {
        public int id {get; set;}
        public string name {get; set;}
        public bool present { get; set; }

        public Student()
        {
            present = false;
        }
        public Student(Student other)
        {
            this.id = other.id;
            this.name = other.name;
           
        }
    }
}
