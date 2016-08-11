using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceHelper
{
    public class HTTPHandler
    {
        Logger log;
        HttpClient client;
        public HTTPHandler(string baseAddress, Logger log = null)
        {
            this.log = log;

            if (this.log == null)
            {
                this.log = new Logger(Logger.LogLevel.None);
            }
           
          
            client = new HttpClient();
            if(baseAddress.LastIndexOf('/') != baseAddress.Length-1)
            {
                baseAddress += '/';
            }
            
            if(baseAddress.IndexOf("http://") == -1 && baseAddress.IndexOf("https://") == -1)
            {
                baseAddress = baseAddress.Insert(0, "http://");
            }
            client.BaseAddress = new Uri(baseAddress);
        }

        public async Task<HttpResponseMessage> MakeGet(string extensionUrl = "")
        {
            extensionUrl = RelativeUrlCheck(extensionUrl);
            HttpResponseMessage response = null;
         
            try
            {
                response = await client.GetAsync(extensionUrl);
                await HandleLogging(response);
            }
            catch(Exception e)
            {
                log.Log(Logger.LogLevel.Errors, e.Message);
                throw e;
            }
            return response;
        }
        private async Task<HttpResponseMessage> HandleLogging(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            log.Log(Logger.LogLevel.Verbose, responseString);
            return response;
        }
        public async Task<HttpResponseMessage> MakePost(FormUrlEncodedContent content, string extensionUrl = "")
        {
            extensionUrl = RelativeUrlCheck(extensionUrl);
            HttpResponseMessage response = null;
         
            try
            {
                response = await client.PostAsync(extensionUrl, content);
                await HandleLogging(response);
            }
            catch (Exception e)
            {
                log.Log(Logger.LogLevel.Errors, e.Message);
                throw e;
            }

            return response;
        }
        public async Task<HttpResponseMessage> MakePost(StringContent content, string extensionUrl = "")
        {
            extensionUrl = RelativeUrlCheck(extensionUrl);
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(extensionUrl, content);
                await HandleLogging(response);
            }
            catch(Exception e)
            {
                log.Log(Logger.LogLevel.Errors, e.Message);
                throw e;
            }

            return response;
        }
       
       public string RelativeUrlCheck(string extensionUrl)
        {
            if (extensionUrl.Length > 0)
            {
                if (extensionUrl[0] == '/')
                {
                    extensionUrl = extensionUrl.Substring(1, extensionUrl.Length - 1);
                }
            }

            return extensionUrl;
        }
    }
}



