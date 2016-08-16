using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceHelper
{
    class AttendanceHandler : BaseHTTPHandler
    {
       
            

        User user;
        string attendanceUrl;
        List<Student> students;



        public AttendanceHandler(string attendanceUrl, User user, HTTPHandler client, Logger log = null)
            : base(client, log)
        {
            this.user = user;
            this.attendanceUrl = attendanceUrl;
        }
        public async Task<HttpResponseMessage> GetAttendance()
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
        private parseStudentList(string toParse)
        {
            students = new List<Student>();

            //SunGard.Tac.Attendance.Init

        }

    }
}
