using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;

namespace PizzaTracker.Controllers
{
    public class PizzaController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        /// <summary>
        /// Get a pizza by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetPizza(int id)
        {
            var pizza = _repo.GetPizzaById(id);
            if (pizza == null)
            {
                return NotFound();
            }
            var order = _repo.GetOrderByPizzaId(id);

            return Ok(new { Pizza = pizza, UserName = order.OrderedBy.UserName, Date = order.Date });
        }
    }
}