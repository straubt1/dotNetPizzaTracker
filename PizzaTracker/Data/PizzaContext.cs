using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using PizzaTracker.Models;

namespace PizzaTracker.Data
{
    public class PizzaContext : DbContext
    {
        public PizzaContext(string connectionString)
            : base(connectionString)
        {

        }
        public PizzaContext()
            : base("PizzaDb")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Crust> Crusts { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Sauce> Sauces { get; set; }
        public DbSet<SauceLevel> SauceLevels { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PizzaQueue> PizzaQueue { get; set; }
        public DbSet<MessageQueue> MessageQueue { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasOptional(a => a.Role)
        //        .WithOptionalDependent()
        //        .WillCascadeOnDelete(true);
        //}
    }
}