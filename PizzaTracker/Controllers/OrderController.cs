using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class OrderController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // GET: api/Order
        public List<Order> GetOrders()
        {
            var list = db.Orders.ToList();
            return list;
        }

        // GET: api/Order/5
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> GetPizza(int id)
        {
            Pizza pizza = await db.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        // POST: api/Order
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> PostPizza(PizzaVm pizzaVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pizza = new Pizza
            {
                CrustId = pizzaVm.Crust.Id,
                SauceId = pizzaVm.Sauce.Id,
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

            var order = new Order
            {
                Date = DateTime.Now,
                Pizzas = new List<Pizza>(),
                OrderEvents = new List<OrderEvent>()
            };

            var oEvent = new OrderEvent
            {
                Date = DateTime.Now,
                EventName = "Order Placed"
            };
            order.Pizzas.Add(pizza);
            order.OrderEvents.Add(oEvent);

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = 1 }, pizza);
        }

        // DELETE: api/Order/5
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> DeletePizza(int id)
        {
            Pizza pizza = await db.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }

            db.Pizzas.Remove(pizza);
            await db.SaveChangesAsync();

            return Ok(pizza);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PizzaExists(int id)
        {
            return db.Pizzas.Count(e => e.Id == id) > 0;
        }
    }
}