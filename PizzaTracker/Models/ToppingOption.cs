using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class ToppingOption
    {
        public virtual int Id { get; set; }
        public int ToppingId { get; set; }
        [ForeignKey("ToppingId")]
        public virtual Topping Topping { get; set; }
        public virtual PizzaSide Side { get; set; }
    }
}