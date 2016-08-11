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

    public class LoginFailed : PostFailed
    {
        public LoginFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
    public class SetSessionFailed : PostFailed
    {
        public SetSessionFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
    public class SubmitAttendanceFailed : PostFailed
    {
        public SubmitAttendanceFailed(HttpResponseMessage r)
            :base(r)
        {

        }
    }
}
