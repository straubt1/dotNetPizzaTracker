using System.Collections.Generic;
using System.Linq;
using PizzaTracker.Code;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<PizzaTracker.Data.PizzaContext>
    {
        private List<Crust> _crusts = new List<Crust>
        {
            new Crust {Name = "Thin", Cost = 11.2},
            new Crust {Name = "Hand Tossed", Cost = 11.1},
            new Crust {Name = "Deep Dish", Cost = 11.3},
        };

        private List<Topping> _toppings = new List<Topping>
        {
            new Topping {Name = "Pepperoni", Cost = 1.2},
            new Topping {Name = "Sausage", Cost = 1.1},
            new Topping {Name = "Chicken", Cost = 1.3},
        };

        private List<Size> _sizes = new List<Size>
        {
            new Size {Name = "Small",Width = 6},
            new Size {Name = "Medium",Width = 10},
            new Size {Name = "Large",Width = 14},
        };

        private List<Sauce> _sauces = new List<Sauce>
        {
            new Sauce {Name = "Red"},
            new Sauce {Name = "White"},
            new Sauce {Name = "None"}
        };

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PizzaContext context)
        {
            PopulateUsers(context);

            PopulateLookups(context);
            context.SaveChanges();
            PopulatePizzas(context);
        }

        private void PopulateUsers(PizzaContext context)
        {
           AddUser(context, "tstraub", "Tom", "Straub", "tom@mail.com", "Test@123");
        }

        private void AddUser(PizzaContext context, string username, string first, string last, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                FirstName = first,
                LastName = last,
                Email = email
            };

            var salt = Crypto.GenerateSalt();
            var passwordHash = Crypto.HashPassword(password, salt);
            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;

            context.Users.AddOrUpdate(x => x.UserName, user);
        }
        private void PopulateLookups(PizzaContext context)
        {
            foreach (var crust in _crusts)
            {
                context.Crusts.AddOrUpdate(x => x.Name, crust);
            }

            foreach (var top in _toppings)
            {
                context.Toppings.AddOrUpdate(x => x.Name, top);
            }

            foreach (var size in _sizes)
            {
                context.Sizes.AddOrUpdate(x => x.Name, size);
            }

            foreach (var sauce in _sauces)
            {
                context.Sauces.AddOrUpdate(x => x.Name, sauce);
            }
        }

        private void PopulatePizzas(PizzaContext context)
        {
            var pizza = new Models.Pizza
            {
                Name = "Supreme",
                Cost = 15.4,
                CrustId =1,
                Toppings = new List<ToppingOption>
                {
                    new ToppingOption{ Topping = context.Toppings.ToList()[0], Side = PizzaSide.Full},
                    new ToppingOption{ Topping = context.Toppings.ToList()[1],  Side = PizzaSide.Left}
                },
                SauceId = 1,
                SizeId = 1
            };

            context.Pizzas.AddOrUpdate(x => x.Name, pizza);
        }
    }
}
