using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceHelper
{
    class SettingSelectHandler : BaseHTTPHandler
    {
        string extensionURL;
        public SettingSelectHandler(HTTPHandler client, string extensionURL = "/Account/SetEnvironment/SessionStart", Logger log = null)
            : base(client, log)
        {
            this.extensionURL = extensionURL;

        }
        public async Task<HttpResponseMessage> SettingsPost(FormUrlEncodedContent values = null)
        {
            HttpResponseMessage response = null;
            if (values == null)
            {
                values = new FormUrlEncodedContent(new[]
                   {
                new KeyValuePair<string,string>("EnvironmentConfiguration.SummerSchool","false"),
                new KeyValuePair<string, string>("EnvironmentConfiguration.Database", "10"),
                new KeyValuePair<string, string>("EnvironmentConfiguration.SchoolYear", ""),
                new KeyValuePair<string, string>("EnvironmentConfiguration.UserMayImpersonate", "false"),
                new KeyValuePair<string, string>("okButton", "")
            });
            }
          
                response = await client.MakePost(values, extensionURL);
            //logic to determine if we really succeeded
            // throw new PostFailed(response);
            if (response == null)
            {
                PostFailed error = new PostFailed(response, "Post response null");
                throw error;
            }
            else if (response.RequestMessage.RequestUri.ToString().IndexOf(extensionURL) != -1)
            {
                SetSessionPostFailed error = new SetSessionPostFailed(response, "Login request rejected by authorizor");
                throw error;
            }

            return response;
        }
    }
}
