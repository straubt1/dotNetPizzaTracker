using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class MessageQueue
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// DateTime added to the queue
        /// </summary>
        public virtual DateTime Date { get; set; }
        
        /// <summary>
        /// Once read set this to false so it wont be read again
        /// </summary>
        public virtual bool Active { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        /// The order to which the message is pertaining to
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        /// <summary>
        /// Message title to display to user
        /// </summary>
        public virtual string MessageTitle { get; set; }
        /// <summary>
        /// Message body to display to user
        /// </summary>
        public virtual string MessageBody { get; set; }
    }
}