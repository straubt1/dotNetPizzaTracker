using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class UsersController : ApiController
    {
        private PizzaTrackerRepo _repo = new PizzaTrackerRepo(new PizzaContext());

        /// <summary>
        /// Get all the users in the system
        /// </summary>
        /// <param name="token">Encrypted user token</param>
        /// <returns></returns>
        public IEnumerable<object> GetUsers(string token)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Admin);

            return _repo.GetAllUsers().Select(x => new
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.CellPhone,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Role = x.Role,
                RoleName = x.Role.Name
            }).ToList();
        }

        /// <summary>
        /// Remove the given user from the system
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteUser(string token, int userid)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Admin);
            try
            {
                _repo.RemoveUser(userid);
            }
            catch //ignore errors
            {
            }

            return Ok();
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public IHttpActionResult Put(string token, [FromBody]UserVm userVm)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Admin);

            _repo.UpdateUser(userVm);

            return Ok();
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public IHttpActionResult PostUser(string token, UserVm userVm)
        {
            var user = _repo.GetUserByEncrypted(token, PizzaTrackerRepo.PizzaRole.Admin);

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

            var newUser = _repo.AddUser(userVm);
            return Ok(newUser);
        }
    }
}