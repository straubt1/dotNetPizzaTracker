using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Controllers
{
    public class LoginController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // POST: api/Login
        public IHttpActionResult Post([FromBody]UserVm userVm)
        {
            var userDb = db.Users.FirstOrDefault(x => x.UserName == userVm.UserName);
            if (userDb == null)
            {
                var content = new StringContent("User not found");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "User not found",
                    Content = content
                });
            }

            var salt = userDb.PasswordSalt;
            var password = Crypto.HashPassword(userVm.Password, salt);
            if (password != userDb.PasswordHash)
            {
                var content = new StringContent("Invalid password");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Invalid password",
                    Content = content
                });
            }
            return CreatedAtRoute("DefaultApi", new { id = userDb.Id }, userDb);
        }
    }
}
