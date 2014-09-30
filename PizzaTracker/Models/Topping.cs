namespace PizzaTracker.Models
{
    public class Topping
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }
        public virtual string Category { get; set; }
    }
}