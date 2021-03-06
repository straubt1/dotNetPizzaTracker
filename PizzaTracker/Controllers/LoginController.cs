﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;
using Newtonsoft.Json;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;
using PizzaTracker.ViewModels;

namespace PizzaTracker.Controllers
{
    public class LoginController : ApiController
    {
        private PizzaContext db = new PizzaContext();

        /// <summary>
        /// Login user based on username and password
        /// </summary>
        /// <param name="userVm"></param>
        /// <returns></returns>
        public object Post([FromBody]UserVm userVm)
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

            //if here, login successful
            if (string.IsNullOrEmpty(userDb.LoginToken))
            {// do not regenerate token... unless its null
                userDb.LoginToken = Crypto.GenerateSalt();
            }
            userDb.LoginExpiration = DateTime.UtcNow.AddMinutes(500);
            var loginVm = new LoginVm { UserId = userDb.Id, UserToken = userDb.LoginToken };
            db.Entry(userDb).State = EntityState.Modified;
            db.SaveChanges();

            var loginVmJson = JsonConvert.SerializeObject(loginVm);
            var encrypted = Aes256.Encrypt(loginVmJson);
            return new { Token = encrypted, Name = userDb.FirstName, Role = userDb.Role.Name, Expiration = userDb.LoginExpiration };
            // return CreatedAtRoute("DefaultApi", new { id = userDb.Id }, userDb);
        }
    }
}
