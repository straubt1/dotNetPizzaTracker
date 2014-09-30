using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class PizzaQueue
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// Is the pizza still active in the queue
        /// When a pizza is done, we set this to false for filtering
        /// </summary>
        public virtual bool Active { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        /// The order to be made
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int StatusId { get; set; }
        /// <summary>
        /// Current status of the pizza in the queue
        /// </summary>
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public int? AssignedToId { get; set; }
        /// <summary>
        /// Who the order is assigned to (employee)
        /// </summary>
        [ForeignKey("AssignedToId")]
        public virtual User AssignedTo { get; set; }
    }
}