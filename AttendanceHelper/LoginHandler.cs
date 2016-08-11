using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AttendanceHelper
{

    class LoginHandler : BaseHTTPHandler
    {

        string password;
        string userName;
        string loginUrl;

        


        public LoginHandler(string loginUrl, string password, string username, HTTPHandler client, Logger log = null)
            : base(client, log)
        {
            this.password = password;
            this.userName = username;
            this.loginUrl = loginUrl;
        }
        public async Task<HttpResponseMessage> Login()
        {
            bool loginSuccess = true;
            HttpResponseMessage response = null;
            FormUrlEncodedContent postParams = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("UserName",userName),
                new KeyValuePair<string, string>("Password",password)
            });

            try
            {
                response = await client.MakePost(postParams, loginUrl);
                //logic to determine if we really did log in else throw exception
                // throw new LoginFailed(response);
            }
            catch (Exception e)
            {
                log.Log(Logger.LogLevel.Errors, "Failed to log on");
                throw new PostFailed(response);
            }
            return response;
        }

    }

}
