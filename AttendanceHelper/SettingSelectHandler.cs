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
                new KeyValuePair<string, string>("EnvironmentConfiguration.UserMayImpersonate", "False"),
                new KeyValuePair<string, string>("okButton", "")
            });
            }
            try
            {
                response = await client.MakePost(values, extensionURL);
                //logic to determine if we really succeeded
                // throw new PostFailed(response);
            }
            catch (Exception e)
            {
                log.Log(Logger.LogLevel.Errors, "Failed select environment settings");
                throw new PostFailed(response);
            }
            return response;
        }
    }
}
