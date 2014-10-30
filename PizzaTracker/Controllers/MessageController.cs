using System;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class MessageController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        /// <summary>
        /// Get the next message in the Queue for the given user
        /// This return null if no message
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public MessageQueue Get(string token)
        {
            var user = _repo.GetUserByEncrypted(token);

            return _repo.GetNextMessage(user.Id);
        }

        /// <summary>
        /// Test Endpoint to always return a message once every 10 seconds
        /// </summary>
        /// <returns></returns>
        public MessageQueue Get()
        {
            return (DateTime.Now.Second % 10 == 0)
                ? new MessageQueue
                {
                    Date = DateTime.Now,
                    MessageTitle = "Test Title",
                    MessageBody = "Test Body",
                    Order = _repo.GetOrderById(1)
                }
                : null;
        }
    }
}
