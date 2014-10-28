using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class OrderController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());
        
        // GET: api/Order
        public IEnumerable<Order> GetOrders(string id)
        {
            var user = _repo.GetUserByEncrypted(id);
            var list =  _repo.GetOrdersForUser(user.Id).OrderByDescending(x=>x.Date).ToList();
            var domain = Request.RequestUri.Scheme + Uri.SchemeDelimiter + Request.RequestUri.Authority;
            //var domain = "http://pizzatracker.azurewebsites.net/app";
            foreach (var l in list)
            {
                l.ShareLink = HttpUtility.HtmlEncode(string.Format("{0}/app/#/neworder/{1}", domain, l.Id));
            }
            return list;
        }

        // POST: api/Order
        [ResponseType(typeof(Pizza))]
        public async Task<IHttpActionResult> PostPizza(PizzaVm pizzaVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = pizzaVm.IsAnon ? _repo.GetAnonUserByEmail(pizzaVm.UserToken) : _repo.GetUserByEncrypted(pizzaVm.UserToken);

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

           var order =  _repo.PlaceOrderForUser(user.Id, new List<Pizza> { pizza }, pizzaVm.Notifications, pizzaVm.Instructions);

            var messageCenter = new MessageCenter(_repo);
            messageCenter.PushMessage("Your order has been placed!", "We will update you along the way.", order.Id);

            if (pizzaVm.IsAnon)
            {
                pizzaVm.UserToken = user.LoginToken;
                var loginVm = new LoginVm { UserId = user.Id, UserToken = user.LoginToken };
                var loginVmJson = JsonConvert.SerializeObject(loginVm);
                var encrypted = Aes256.Encrypt(loginVmJson);
                pizzaVm.AnonUser =
                    new
                    {
                        Token = encrypted,
                        Name = user.FirstName,
                        Role = user.Role.Name,
                        Expiration = user.LoginExpiration
                    };
            }
            return CreatedAtRoute("DefaultApi", new { id = 1 }, pizzaVm);
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