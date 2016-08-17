using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace AttendanceHelper
{
    class AttendanceHandler : BaseHTTPHandler
    {
       
            

        User user;
        string attendanceUrl;
        List<Student> students;
        string attendFunName;
        List<string> attributes;

        public AttendanceHandler(string attendanceUrl, User user, HTTPHandler client, Logger log = null)
            : base(client, log)
        {
              
            this.user = user;
            this.attendanceUrl = attendanceUrl;
            this.attendFunName= Properties.Resources.AttendanceFuncName;
        }
        public async Task<HttpResponseMessage> PopulateStudentList()
        {
           
            HttpResponseMessage response = null;

            response = await client.MakeGet(attendanceUrl);
           
            parseStudentList(await response.Content.ReadAsStringAsync());
            if (response == null)
            {
               GetFailed error = new GetFailed(response, "Get attendance response null");
                throw error;
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                GetFailed error = new GetFailed(response, "Response error code not OK");
                throw error;
            }


            return response;
        }
        private void parseStudentList(string toParse)
        {
            students = new List<Student>();
            string studentInfo;
            //this will most likely change. Need a way to make this dynamic, but I won't know exactly what I need until I found out what post parameters are required.
            //For now, just student names will do.
            toParse = toParse.Substring(toParse.IndexOf(attendFunName), toParse.Length - toParse.IndexOf(attendFunName));
            studentInfo = toParse;
            toParse = toParse.Replace("\"", string.Empty);
            var studentStringList = toParse.Split(',').ToList();

            //Eventually split by studentID and get students by themselves here
            List<List<string>> studentUnparsedList;
            

            //this loop will be run once per student per desired attribute
            foreach(string attribute in studentStringList)
            {
                if(attribute.IndexOf("StudentName:")!=-1)
                {
                    string lastName = attribute.Substring(attribute.IndexOf(':')+1);
                    string FirstName = studentStringList[studentStringList.IndexOf(attribute) + 1];
                    FirstName = FirstName.Substring(0, FirstName.LastIndexOf(' '));
                    FirstName.Replace(" ", "");
                    Student student = new Student();
                    student.name = FirstName + " " + lastName;
                    students.Add(student);
                };
            }
            //TODO: Find a way to do this dynamically. Find a way to make this multi attribute friendly
            
        }
        public List<Student> GetStudentList()
        {
            return students;
        }

    }
}
 