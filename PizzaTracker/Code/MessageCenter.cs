using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PizzaTracker.Models;

namespace PizzaTracker.Code
{
    public class MessageCenter
    {
        private readonly PizzaTrackerRepo _repo;

        public MessageCenter(PizzaTrackerRepo repo)
        {
            _repo = repo;
        }

        public void PushMessage(string title, string message, int orderId)
        {
            var order = _repo.GetOrderById(orderId);
            if (order == null)
            {
                return;
            }
            if (order.NotificationPush)
            {
                _repo.AddMessageQueue(orderId, title, message);
            }
            if(order.NotificationEmail)
            {
                EmailMessenger.Send(order.OrderedBy.Email, title,message);
            }
            if (order.NotificationPush)
            {
                SmsMessenger.Send("5132529466", title, message);
            }
        }
    }
}