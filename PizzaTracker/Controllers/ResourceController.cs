using System.Linq;
using System.Web.Http;
using PizzaTracker.Data;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class ResourceController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        /// <summary>
        /// Get all the pizza resource in one shot
        /// </summary>
        /// <returns></returns>
        public ResourceVm Get()
        {
            var all = new ResourceVm
            {
                Crusts = db.Crusts.ToList(),
                Sauces = db.Sauces.ToList(),
                Sizes = db.Sizes.ToList(),
                Toppings = db.Toppings.ToList(),
                Statuses =  db.Statuses.ToList(),
                SauceLevels =  db.SauceLevels.ToList()
            };

            return all;
        }
    }
}
