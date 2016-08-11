using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AttendanceHelper
{
    public class PostFailed : Exception
    {
        bool IsSuccessCode;
        HttpStatusCode Status;
        string Body;

        public PostFailed(HttpResponseMessage r)
        {
            IsSuccessCode = r.IsSuccessStatusCode;
            Status = r.StatusCode;
            Body = r.Content.ReadAsStringAsync().Result;
        }
    }

    public class LoginPostFailed : PostFailed
    {
        public LoginPostFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
    public class SetSessionPostFailed : PostFailed
    {
        public SetSessionPostFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
    public class SubmitAttendancePostFailed : PostFailed
    {
        public SubmitAttendancePostFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
}
