using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class QueueController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // GET: api/Order
        public List<Queue> GetQueue()
        {
           // var user = UserInfo.GetUserInfo(db, id);

            var list = db.Queues.ToList();//filter
            return list;
        }

        public Queue PostQueue(int id)
        {
            var q = db.Queues.FirstOrDefault(x => x.Id == id);
            q.StatusId++;
            db.Entry(q).State = EntityState.Modified;
            db.SaveChanges();

            return q;
        }
    }
}
