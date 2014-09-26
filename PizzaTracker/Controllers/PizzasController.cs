using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class PizzasController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // GET: api/Pizzas
        public List<Pizza> GetPizzas()
        {
            var p = db.Pizzas.ToList();
            return p;
        }

        // GET: api/Pizzas/5
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

        // PUT: api/Pizzas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPizza(int id, Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pizza.Id)
            {
                return BadRequest();
            }

            db.Entry(pizza).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pizzas
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> PostPizza(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pizzas.Add(pizza);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pizza.Id }, pizza);
        }

        // DELETE: api/Pizzas/5
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