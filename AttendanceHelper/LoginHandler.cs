using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AttendanceHelper
{
    //Handles user credentials and logging in.
    class LoginHandler : BaseHTTPHandler
    {
        User user;
        string loginUrl;




        public LoginHandler(string loginUrl, User user, HTTPHandler client, Logger log = null)
            : base(client, log)
        {
            this.user = user;
            this.loginUrl = loginUrl;
        }
        public async Task<HttpResponseMessage> Login()
        {
            HttpResponseMessage response = null;
            FormUrlEncodedContent postParams = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserName",user.username),
                new KeyValuePair<string, string>("Password",user.password)
            });


            response = await client.MakePost(postParams, loginUrl);
            if (response == null)
            {
                PostFailed error = new PostFailed(response, "Post response null");
                throw error;
            }
            else if (response.RequestMessage.RequestUri.ToString().IndexOf(loginUrl) != -1)
            {
                LoginPostFailed error = new LoginPostFailed(response, "Login request rejected by authorizor");
                throw error;
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                LoginPostFailed error = new LoginPostFailed(response, "Web Status not ok");
                throw error;
            }


            return response;
        }

    }

}
