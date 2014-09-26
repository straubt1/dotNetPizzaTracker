using System.Collections.Generic;

namespace PizzaTracker.Models
{
    public class ResourceVm
    {
        public List<Crust> Crusts { get; set; }
        public List<Topping> Toppings { get; set; }
        public List<Sauce> Sauces { get; set; }
        public List<Size> Sizes { get; set; }
    }
}