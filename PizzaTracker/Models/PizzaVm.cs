using System.Collections.Generic;

namespace PizzaTracker.Models
{
    public class PizzaVm
    {
        public Crust Crust { get; set; }
        public List<Topping> Toppings { get; set; }
        public Sauce Sauce { get; set; }
        public Size Size { get; set; }
    }
}