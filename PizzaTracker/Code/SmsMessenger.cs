using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Twilio;

namespace PizzaTracker.Code
{
    public class SmsMessenger
    {
        public Message Send(string number, string message, List<string> images = null)
        {
            //number = CleanPhoneNumber(number);
            const string accountSid = "AC6207f279e631ef1322fdcebc0d0d8865";
            const string authToken = "4f4f4fed9b023650fd5e370806da9f3b";
            const string twilioNumber = "+15139724919";
            var twilio = new TwilioRestClient(accountSid, authToken);

            Message result;
            
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

        private string CleanPhoneNumber(string original)
        {
            return original.StartsWith("+") ? original : string.Format("+{0}", original);
        }
    }
}