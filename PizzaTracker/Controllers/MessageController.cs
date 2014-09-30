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

        // GET: api/Message
        public Message Get()
        {
            
            return (DateTime.Now.Second % 10 == 0)
                ? new Message
                {
                    Date = DateTime.Now,
                    Display = "Message about your order",
                    Status = "Delivered"
                }
                : null;
        }

        // GET: api/Message/5
        public MessageQueue Get(string id)
        {
            var user = _repo.GetUserByEncrypted(id);
            var message = _repo.GetNextMessage(user.Id);
            return message;
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
        }
    }
}
