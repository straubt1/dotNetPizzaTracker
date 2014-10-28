using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class UsersController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        // GET: api/Users
        public IEnumerable<object> GetUsers()
        {
            //return db.Users.ToList();
            return db.Users.Select(x => new
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Role = x.Role
            }).ToList();
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, UserVm userVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = userVm.UserName,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                Email = userVm.Email,
                RoleId = 1,
                PasswordHash = "hash",
                PasswordSalt = "salt",
                PasswordResetToken = "null",
                LoginToken = string.Empty
            };

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserVm userVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Users.FirstOrDefault(x => x.UserName == userVm.UserName) != null)
            {
                var content = new StringContent("User already exists");
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    ReasonPhrase = "User already exists",
                    Content = content
                });
            }
            var salt = Crypto.GenerateSalt();
            var password = Crypto.HashPassword(userVm.Password, salt);
            var user = new User
            {
                UserName = userVm.UserName,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                Email = userVm.Email,
                RoleId = 3,//customer
                PasswordHash = password,
                PasswordSalt = salt,
                PasswordResetToken = "null",
                LoginToken = string.Empty
            };

            db.Users.AddOrUpdate(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}