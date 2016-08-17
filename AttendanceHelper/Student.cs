using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceHelper
{
    class Student
    {
        public int id {get; set;}
        public string name {get; set;}
      

        public Student()
        {

        }
        public Student(Student other)
        {
            this.id = other.id;
            this.name = other.name;
           
        }
    }
}
