using System;
using System.Collections.Generic;
using RestSharp;

namespace PizzaTracker.Code
{
    public static class EmailMessenger
    {
        public static void Send(string email, string title, string message, List<string> images = null)
        {
            try
            {
                SendMessage(email, title, message);
            }
            catch
            {
            }
        }

        private static IRestResponse SendMessage(string to, string subject, string body)
        {
            var client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-f225d86c1fd3cb001e52b57ba8ca2e79");
            var request = new RestRequest();
            request.AddParameter("domain",
                                 "sandboxa4ad660531e24345adc4853ff2007cd6.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Pizza Tracker <pizza@orderme.com>");
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}