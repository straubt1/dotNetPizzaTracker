using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class EmailController : ApiController
    {
        // POST: api/Order
        //[ResponseType(typeof (Pizza))]
        public async Task<IHttpActionResult> PostEmail(EmailVm emailVm)
        {
            EmailMessenger.Send(emailVm.to, emailVm.subject, emailVm.message);
            return CreatedAtRoute("DefaultApi", new { id = 1 }, emailVm);
        }
    }
}