using System.Collections.Generic;

namespace PizzaTracker.Models
{
    public class PizzaVm
    {
        public string UserToken { get; set; }
        public Crust Crust { get; set; }
        public List<Topping> Toppings { get; set; }
        public Sauce Sauce { get; set; }
        public SauceLevel SauceLevel { get; set; }
        public Size Size { get; set; }
        public string Instructions { get; set; }
    }
}