using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class RegisterController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public object Post([FromBody]UserVm userVm)
        {
            if (_repo.GetUserByUserName(userVm.UserName) != null)
            {
                var content = new StringContent("User already exists");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    ReasonPhrase = "User already exists",
                    Content = content
                });
            }

            var userDb = _repo.AddUser(userVm);

            if (userDb == null)
            {
                var content = new StringContent("User not found even though I just added it");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "User not found even though I just added it",
                    Content = content
                });
            }

            var loginVm = new LoginVm { UserId = userDb.Id, UserToken = userDb.LoginToken };
            var loginVmJson = JsonConvert.SerializeObject(loginVm);
            var encrypted = Aes256.Encrypt(loginVmJson);

            return new { Token = encrypted, Name = userDb.FirstName, Role = userDb.Role.Name, Expiration = userDb.LoginExpiration };
        }
    }
}