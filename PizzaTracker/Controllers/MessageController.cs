using System;
using System.Web.Http;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class MessageController : ApiController
    {
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
        public string Get(int id)
        {
            return "value";
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
