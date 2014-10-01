using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Web.WebSockets;
using Newtonsoft.Json;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Code
{
    public class PizzaTrackerRepo
    {
        public enum PizzaStatus
        {
            Received = 1,
            Creating = 2,
            Baking = 3,
            Preparing = 4,
            Ready = 5
        }

        private readonly PizzaContext _context;

        public PizzaTrackerRepo(PizzaContext context)
        {
            _context = new PizzaContext();
        }

        #region Users
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }
        public User GetUserByEncrypted(string encrypted)
        {
            var decrypted = Aes256.Decrypt(encrypted);
            var loginVm = JsonConvert.DeserializeObject<LoginVm>(decrypted);

            var userDb = GetUserById(loginVm.UserId);
            if (userDb == null)
            { throw new AuthenticationException("User Id not found: " + loginVm.UserId); }

            if (userDb.LoginToken != loginVm.UserToken)
            {
                throw new AuthenticationException("User Token not valid: " + loginVm.UserToken);
            }

            if (userDb.LoginExpiration < PizzaTime.Now)
            {
                throw new AuthenticationException("User Token Expired: " + userDb.LoginExpiration);
            }

            return userDb;
        }
        #endregion

        public Order GetOrderById(int id)
        {
            return _context.Orders.SingleOrDefault(x => x.Id == id);
        }

        public Pizza GetPizzaById(int id)
        {
            return _context.Pizzas.SingleOrDefault(x => x.Id == id);
        }

        public PizzaQueue GetPizzaQueueById(int id)
        {
            return _context.PizzaQueue.Single(x => x.Id == id);
        }

        public IEnumerable<PizzaQueue> GetpPizzaQueues()
        {
            return _context.PizzaQueue.Where(x=>x.Active);
        }

        /// <summary>
        /// Get all the orders for a user (Customer)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetOrdersForUser(int userId)
        {
            return _context.Orders.Where(x => x.OrderedById == userId && x.Show);
        }

        public Order PlaceOrderForUser(int userId, List<Pizza> pizzas, string customInst = null)
        {
            var orderEvent = new OrderEvent
            {
                Date = PizzaTime.Now,
                Description = "Order was placed by userId: " + userId
            };
            var order = new Order
            {
                Date = PizzaTime.Now,
                OrderEvents = new List<OrderEvent> { orderEvent },
                OrderedById = userId,
                Pizzas = pizzas,
                Show = true,//default
                CustomInstructions = customInst
            };

            //add order to database
            order = _context.Orders.Add(order);
            _context.SaveChanges();

            //add to pizza queue
            var pizzaQ = new PizzaQueue
            {
                AssignedToId = null,
                OrderId = order.Id,
                StatusId = (int)PizzaStatus.Received,
                Active = true//default
            };
            _context.PizzaQueue.Add(pizzaQ);

            //add to message queue
            var messageQ = new MessageQueue
            {
                OrderId = order.Id,
                Date = PizzaTime.Now,
                MessageTitle = "Your order has been placed!",
                MessageBody = "We will update you along the way.",
                Active = true//default
            };
            _context.MessageQueue.Add(messageQ);
            _context.SaveChanges();

            return order;
        }

        /// <summary>
        /// Sets an orders show value to true or false
        /// This will hide it on the users orders screen
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="showValue"></param>
        /// <returns></returns>
        public Order SetOrderShow(int orderId, bool showValue)
        {
            var order = GetOrderById(orderId);
            var orderEvent = new OrderEvent
            {
                Date = PizzaTime.Now,
                Description = "Order property Show was set to: " + showValue
            };
            order.OrderEvents.Add(orderEvent);

            order.Show = showValue;
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
            return order;
        }

        /// <summary>
        /// Sets a pizza queue active value to true or false
        /// This will hide it on the users queue screen
        /// </summary>
        /// <param name="pizzaQueueId"></param>
        /// <param name="activeValue"></param>
        /// <returns></returns>
        public PizzaQueue SetPizzaQueueActive(int pizzaQueueId, bool activeValue)
        {
            var pizzaQ = GetPizzaQueueById(pizzaQueueId);
            var order = GetOrderById(pizzaQ.OrderId);
            var orderEvent = new OrderEvent
            {
                Date = PizzaTime.Now,
                Description = "Order property Active was set to: " + activeValue
            };
            order.OrderEvents.Add(orderEvent);

            pizzaQ.Active = activeValue;
            _context.Entry(order).State = EntityState.Modified;
            _context.Entry(pizzaQ).State = EntityState.Modified;
            _context.SaveChanges();

            AddMessageQueue(pizzaQ.OrderId, "Pizza was removed", "Your pizza was removed from queue");
            return pizzaQ;
        }

        /// <summary>
        /// Get next message in the queue if any
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MessageQueue GetNextMessage(int userId)
        {
            var message = _context.MessageQueue.OrderBy(x => x.Date).FirstOrDefault(x => x.Order.OrderedById == userId && x.Active);
            if (message == null)
            {
                return null;
            }

            //update to read
            message.Active = false;
            _context.Entry(message).State = EntityState.Modified;
            _context.SaveChanges();

            return message;
        }

        /// <summary>
        /// Add a message for an order to the queue
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="msgTitle"></param>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        public MessageQueue AddMessageQueue(int orderId, string msgTitle, string msgBody)
        {
            var messageQ = new MessageQueue
            {
                Date = PizzaTime.Now,
                OrderId = orderId,
                MessageTitle = msgTitle,
                MessageBody = msgBody,
                Active = true
            };

            messageQ =_context.MessageQueue.Add(messageQ);
            _context.SaveChanges();
            return messageQ;
        }

        /// <summary>
        /// Update the status of the pizza queue
        /// Include message title and body
        /// </summary>
        /// <param name="pizzaQueueId"></param>
        /// <param name="newStatus"></param>
        /// <param name="msgTitle"></param>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        public PizzaQueue UpdatePizzaQueue(int pizzaQueueId, PizzaStatus newStatus, string msgTitle, string msgBody)
        {
            var pizzaQ = GetPizzaQueueById(pizzaQueueId);
            var order = GetOrderById(pizzaQ.OrderId);
            //update status
            pizzaQ.StatusId = (int)newStatus;
            _context.Entry(pizzaQ).State = EntityState.Modified;

            //add event
            var orderEvent = new OrderEvent
            {
                Date = PizzaTime.Now,
                Description = "Order Status was set to: " + newStatus
            };
            order.OrderEvents.Add(orderEvent);
            _context.Entry(order).State = EntityState.Modified;

            _context.SaveChanges();

            AddMessageQueue(pizzaQ.OrderId, msgTitle, msgBody);
            return pizzaQ;
        }
    }
}