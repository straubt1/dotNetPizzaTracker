using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public enum PizzaSide {Full, Left, Right}

    public class Crust
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }
    }

    public class Topping
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }
        public virtual string Category { get; set; }
    }

    public class ToppingOption
    {
        public virtual int Id { get; set; }
        public int ToppingId { get; set; }
        [ForeignKey("ToppingId")]
        public virtual Topping Topping { get; set; }
        public virtual PizzaSide Side { get; set; }
    }

    public class Sauce
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Size
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Width { get; set; }
    }

    public class Pizza
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }

        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
        public int CrustId { get; set; }
        [ForeignKey("CrustId")]
        public virtual Crust Crust { get; set; }
        public virtual List<ToppingOption> Toppings { get; set; }
        public int SauceId { get; set; }
        [ForeignKey("SauceId")]
        public virtual Sauce Sauce { get; set; }
    }


    public class Order
    {
        public virtual int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<OrderEvent> OrderEvents { get; set; }
        public virtual List<Pizza> Pizzas { get; set; }

        public int OrderedById { get; set; }
        [ForeignKey("OrderedById")]
        public virtual User OrderedBy { get; set; }

        public virtual bool Show { get; set; }
    }

    public class OrderEvent
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string EventName { get; set; }
    }

    public class User
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string PasswordResetToken { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual string LoginToken { get; set; }
        public virtual DateTime? LoginExpiration { get; set; }
    }

    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Status
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Queue
    {
        public virtual int Id { get; set; }
        public virtual string Message { get; set; }
        public virtual bool Active { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public int? AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public virtual User AssignedTo { get; set; }
    }
}