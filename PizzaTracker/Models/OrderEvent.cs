using System;

namespace PizzaTracker.Models
{
    public class OrderEvent
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// DateTime of event
        /// </summary>
        public virtual DateTime Date { get; set; }
        /// <summary>
        /// Description of the event
        /// </summary>
        public virtual string Description { get; set; }
    }
}