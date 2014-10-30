using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
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
        
        /// <summary>
        /// Get all active orders for a user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetOrders(string token)
        {
            var user = _repo.GetUserByEncrypted(token);
            var list =  _repo.GetOrdersForUser(user.Id).OrderByDescending(x=>x.Date).ToList();
            var domain = Request.RequestUri.Scheme + Uri.SchemeDelimiter + Request.RequestUri.Authority;
            //var domain = "http://pizzatracker.azurewebsites.net/app";
            foreach (var l in list)
            {
                l.ShareLink = HttpUtility.HtmlEncode(string.Format("{0}/app/#/neworder/{1}", domain, l.Id));
            }
            return list;
        }

        /// <summary>
        /// Add a pizza/order to the system
        /// </summary>
        /// <param name="pizzaVm"></param>
        /// <returns></returns>
        public IHttpActionResult PostPizza(PizzaVm pizzaVm)
        {
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

        /// <summary>
        /// Set an order to inactive 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public IHttpActionResult DeletePizza(string token, int orderid)
        {
            var user = _repo.GetUserByEncrypted(token);

            _repo.SetOrderShow(orderid, false);

            return Ok();
        }
    }
}