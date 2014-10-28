using System.Collections.Generic;
using PizzaTracker.Models;

namespace PizzaTracker.ViewModels
{
    public class PizzaVm
    {
        public string UserToken { get; set; }
        public bool IsAnon { get; set; }
        public object AnonUser { get; set; }
        public NotificationVm Notifications { get; set; }

        public Crust Crust { get; set; }
        public List<Topping> Toppings { get; set; }
        public Sauce Sauce { get; set; }
        public SauceLevel SauceLevel { get; set; }
        public Size Size { get; set; }
        public string Instructions { get; set; }
    }
}