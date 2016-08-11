using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceHelper
{
    class Student
    {
        int id {get; set;}
        string name {get; set;}
        int homeroom {get; set;}

        public Student()
        {

        }
        public Student(Student other)
        {
            this.id = other.id;
            this.name = other.name;
            this.homeroom = other.homeroom;
        }
    }
}
