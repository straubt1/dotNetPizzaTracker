using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class OrderController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        // GET: api/Order
        public IEnumerable<Order> GetOrders(string id)
        {
            var user = _repo.GetUserByEncrypted(id);
            return _repo.GetOrdersForUser(user.Id).ToList();
        }

        // POST: api/Order
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> PostPizza(PizzaVm pizzaVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _repo.GetUserByEncrypted(pizzaVm.UserToken);

            var pizza = new Pizza
            {
                CrustId = pizzaVm.Crust.Id,
                Sauce = new SauceOption { SauceId = pizzaVm.Sauce.Id, SauceLevelId = pizzaVm.SauceLevel.Id},
                SizeId = pizzaVm.Size.Id,
                Toppings = new List<ToppingOption>()
            };
            foreach (var top in pizzaVm.Toppings)
            {
                var tOption = new ToppingOption
                {
                    ToppingId = top.Id,
                    Side = PizzaSide.Full
                };
                pizza.Toppings.Add(tOption);
            }

            _repo.PlaceOrderForUser(user.Id, new List<Pizza> { pizza }, pizzaVm.Instructions);

            return CreatedAtRoute("DefaultApi", new { id = 1 }, pizza);
        }

        // DELETE: api/Order/5
        //[ResponseType(typeof(Pizza))]
        public IHttpActionResult DeletePizza(int id)
        {
            _repo.SetOrderShow(id, false);

            return Ok();
        }
    }
}