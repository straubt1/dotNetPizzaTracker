using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class EmailController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        /// <summary>
        /// Send a share email
        /// </summary>
        /// <param name="emailVm"></param>
        /// <returns></returns>
        public IHttpActionResult PostEmail(EmailVm emailVm)
        {
            var order = _repo.GetOrderById(emailVm.orderid);
            if (order == null)
            {
                var content = new StringContent("Order Not Found");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "Order Not Found",
                    Content = content
                });
            }
            var pizza = order.Pizzas.First();
            var subject = string.Format("{0} wants to share a Pizza with you! - PizzaTracker", emailVm.user);
            var toppingsLine = pizza.Toppings.Any()
                ? string.Join(", ", pizza.Toppings.Select(x=>x.Topping.Name))
                : "No Toppings on this pizza, are you crazy? or just boring...";

            var body = string.Format(@"<html>Hey!<br/>Check out this pizza <a href='{0}'>My Awesome Pizza</a><br/><br/>
                        <div>
                            <span>
                                <strong>{1}</strong> / <strong>{2}</strong> / <strong>{3} ({4})</strong>
                            </span>
                            <br>
                            <div>
                                <i>{5}</i>
                            </div>
                        </div>
                        </html>",
                        emailVm.link,
                        pizza.Size.Name,
                        pizza.Crust.Name,
                        pizza.Sauce.Sauce.Name,
                        pizza.Sauce.SauceLevel.Name,
                        toppingsLine);

            EmailMessenger.Send(emailVm.to, subject, body);
            return CreatedAtRoute("DefaultApi", new { id = 1 }, emailVm);
        }
    }
}