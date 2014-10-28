using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class Order
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// Date order was placed
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The events that occurred to make the pizza
        /// </summary>
        public virtual List<OrderEvent> OrderEvents { get; set; }
        /// <summary>
        /// Pizzas ordered
        /// </summary>
        public virtual List<Pizza> Pizzas { get; set; }

        public int OrderedById { get; set; }
        /// <summary>
        /// Who ordered the pizza
        /// </summary>
        [ForeignKey("OrderedById")]
        public virtual User OrderedBy { get; set; }

        /// <summary>
        /// Custom instructions given by the customer
        /// </summary>
        public virtual string CustomInstructions { get; set; }
        /// <summary>
        /// If we should show this order to a customer
        /// They can 'remove' the order, but it just hides it
        /// </summary>
        public virtual bool Show { get; set; }

        public bool NotificationEmail { get; set; }
        public bool NotificationText { get; set; }
        public bool NotificationPush { get; set; }

        [NotMapped]
        public string ShareLink { get; set; }
    }
}