using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Twilio;

namespace PizzaTracker.Code
{
    public static class SmsMessenger
    {
        public static Message Send(string number, string title, string body, List<string> images = null)
        {
            try
            {
                //number = CleanPhoneNumber(number);
                const string accountSid = "AC6207f279e631ef1322fdcebc0d0d8865";
                const string authToken = "4f4f4fed9b023650fd5e370806da9f3b";
                const string twilioNumber = "+15139724919";
                var twilio = new TwilioRestClient(accountSid, authToken);

                Message result;
                var message = string.Format("{0}\r\n{1}", title, body);
                if (images == null || images.Count == 0)
                {
                    result = twilio.SendMessage(twilioNumber,
                        number,
                        message);
                }
                else
                {
                    result = twilio.SendMessage(twilioNumber,
                        number,
                        message,
                        images.ToArray());
                }

                return result;
            }
            catch
            {
                //do not throw - if this fails, so what...
                return null;
            }
        }

        private static string CleanPhoneNumber(string original)
        {
            return original.StartsWith("+") ? original : string.Format("+{0}", original);
        }
    }
}