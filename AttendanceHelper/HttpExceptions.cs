﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AttendanceHelper
{
    //The idea of these exceptions is to catch the most specific ones first and then allow them to bubble up to more general catches.
    public class GetFailed : Exception
    {
        public readonly HttpResponseMessage reseponse = null;

        public GetFailed(HttpResponseMessage r, string message = "")
            :base(message)
        {
            reseponse = r;
        }
    }
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
