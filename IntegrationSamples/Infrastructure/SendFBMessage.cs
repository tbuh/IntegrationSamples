using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace IntegrationSamples.Infrastructure
{
    public class SendFBMessage
    {
        public void Send(string userId, string message)
        {
            var json = $@" {{recipient: {{  id: {userId}}},message: {{text: ""{message}"" }}}}";

            PostRaw($"https://graph.facebook.com/v2.6/me/messages?access_token={AppSettings.access_token}", json);
        }

        private string PostRaw(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
    }
}