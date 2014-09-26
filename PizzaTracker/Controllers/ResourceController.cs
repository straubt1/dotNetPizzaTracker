using System.Linq;
using System.Web.Http;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class ResourceController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // GET: api/Resource
        public ResourceVm Get()
        {
            var all = new ResourceVm
            {
                Crusts = db.Crusts.ToList(),
                Sauces = db.Sauces.ToList(),
                Sizes = db.Sizes.ToList(),
                Toppings = db.Toppings.ToList()
            };

            return all;
        }

        // GET: api/Resource/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Resource
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Resource/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Resource/5
        public void Delete(int id)
        {
        }
    }
}
