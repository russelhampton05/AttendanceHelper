using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AttendanceHelper
{
    public class PostFailed : Exception
    {
   
       
        public readonly HttpResponseMessage reseponse = null;

        public PostFailed(HttpResponseMessage r, string message = "")
            :base(message)
        {
            reseponse = r;
        }
    }

    public class LoginPostFailed : PostFailed
    {
        public LoginPostFailed(HttpResponseMessage r, string message = "")
            :base(r, message)
        {

        }
    }
    public class SetSessionPostFailed : PostFailed
    {
        public SetSessionPostFailed(HttpResponseMessage r, string message = "")
            :base(r, message)
        {

        }
    }
    public class SubmitAttendancePostFailed : PostFailed
    {
        public SubmitAttendancePostFailed(HttpResponseMessage r, string message = "")
            :base(r, message)
        {

        }
    }
}
