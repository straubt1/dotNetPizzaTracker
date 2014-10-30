using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

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

        public enum PizzaRole
        {
            Admin = 1,
            Employee = 2,
            Customer = 3
        }

        private readonly PizzaContext _context;

        public PizzaTrackerRepo(PizzaContext context)
        {
            _context = new PizzaContext();
        }

        #region Users
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        /// <summary>
        /// Remove the user if they exist
        /// </summary>
        /// <param name="id"></param>
        public void RemoveUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Update a user if they exist
        /// </summary>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public User UpdateUser(UserVm userVm)
        {
            var user = GetUserById(userVm.Id);
            if (user != null)
            {
                user.FirstName = userVm.FirstName;
                user.LastName = userVm.LastName;
                user.Email = userVm.Email;
                user.CellPhone = userVm.PhoneNumber;

                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return user;
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }
        public User GetUserByUserName(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == username);
        }
        /// <summary>
        /// Get the user based on encrypted string and optional role 
        /// Throws exception on bad token
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public User GetUserByEncrypted(string encrypted, PizzaRole? role = null)
        {
            var decrypted = Aes256.Decrypt(encrypted);
            var loginVm = JsonConvert.DeserializeObject<LoginVm>(decrypted);

            var userDb = GetUserById(loginVm.UserId);
            if (userDb == null)
            {
                throw new AuthenticationException("User Id not found: " + loginVm.UserId);
            }
            
            if (userDb.LoginToken != loginVm.UserToken)
            {
                throw new AuthenticationException("User Token not valid: " + loginVm.UserToken);
            }

            if (userDb.LoginExpiration < PizzaTime.Now)
            {
                throw new AuthenticationException("User Token Expired: " + userDb.LoginExpiration);
            }

            if (role != null && userDb.RoleId > (int)role)//lower role means more privilege
            {
                throw new AuthenticationException("User Role Not Sufficient: " + userDb.Role.Name);
            }

            return userDb;
        }
        public User GetAnonUserByEmail(string userEmail)
        {
            var existing = _context.Users.FirstOrDefault(x => x.UserName == userEmail);
            if (existing != null)
            {
                existing.LoginToken = Crypto.GenerateSalt();
                existing.LoginExpiration = DateTime.UtcNow.AddMinutes(500);
                _context.Entry(existing).State = EntityState.Modified;
                _context.SaveChanges();
                return existing;
            }

            //create random password, this will never be used to login directly
            var salt = Crypto.GenerateSalt();
            var password = Crypto.HashPassword(salt, salt);
            var role = _context.Roles.FirstOrDefault(x => x.Id == 3);
            var user = new User
            {
                //use email as username since we wont ask for their info
                UserName = userEmail,
                FirstName = "Guest",
                //LastName = userVm.LastName,
                Email = userEmail,
                Role = role,
                PasswordHash = password,
                PasswordSalt = salt,
                PasswordResetToken = "null",
                LoginToken = Crypto.GenerateSalt(),
                LoginExpiration = DateTime.UtcNow.AddMinutes(500)
            };
            //userDb.LoginToken = Crypto.GenerateSalt();
            //userDb.LoginExpiration = DateTime.UtcNow.AddMinutes(500);
            //var loginVm = new LoginVm { UserId = userDb.Id, UserToken = userDb.LoginToken };
            //db.Entry(userDb).State = EntityState.Modified;

            _context.Users.AddOrUpdate(x => x.Email, user);
            _context.SaveChanges();
            var u = _context.Users.ToList().FirstOrDefault(x => x.UserName == userEmail);
            return u;
        }

        /// <summary>
        /// Add user to the system
        /// </summary>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public User AddUser(UserVm userVm, PizzaRole roleId = PizzaRole.Customer)
        {
            var salt = Crypto.GenerateSalt();
            var password = Crypto.HashPassword(userVm.Password, salt);
            var role = _context.Roles.FirstOrDefault(x => x.Id == (int)roleId);
            var user = new User
            {
                UserName = userVm.UserName,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                Email = userVm.Email,
                Role = role,
                PasswordHash = password,
                PasswordSalt = salt,
                PasswordResetToken = "null",
                LoginToken = Crypto.GenerateSalt(),
                LoginExpiration = DateTime.UtcNow.AddMinutes(500)
            };

            _context.Users.AddOrUpdate(user);
            _context.SaveChanges();

            return GetUserByUserName(userVm.UserName);

        }
        #endregion

        public Order GetOrderById(int id)
        {
            return _context.Orders.SingleOrDefault(x => x.Id == id);
        }

        public Order GetOrderByPizzaId(int id)
        {
            return _context.Orders.SingleOrDefault(x => x.Pizzas.Any(y => y.Id == id));
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
            return _context.PizzaQueue.Where(x => x.Active);
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

        public Order PlaceOrderForUser(int userId, List<Pizza> pizzas, NotificationVm notifications, string customInst = null)
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
                CustomInstructions = customInst,
                NotificationEmail = notifications.Email,
                NotificationText = notifications.Text,
                NotificationPush = notifications.Push
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

            //new MessageCenter(this).PushMessage("Pizza was removed", "Your pizza was removed from queue", pizzaQ.OrderId);
            return pizzaQ;
        }

        /// <summary>
        /// Get next message in the queue if any
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MessageQueue GetNextMessage(int userId)
        {
            //todo filter out messages that are older than 10 minutes
            var message = _context.MessageQueue.OrderBy(x => x.Date).FirstOrDefault(x => x.Order.OrderedById == userId && x.Active);// && (DateTime.Now - x.Date).TotalMinutes < 10);
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

            messageQ = _context.MessageQueue.Add(messageQ);
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
            new MessageCenter(this).PushMessage(msgTitle, msgBody, pizzaQ.OrderId);
            //AddMessageQueue(pizzaQ.OrderId, msgTitle, msgBody);
            return pizzaQ;
        }
    }
}