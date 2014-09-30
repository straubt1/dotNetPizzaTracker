using System.Collections.Generic;
using PizzaTracker.Models;

namespace PizzaTracker.ViewModels
{
    public class ResourceVm
    {
        public List<Crust> Crusts { get; set; }
        public List<Topping> Toppings { get; set; }
        public List<Sauce> Sauces { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Status> Statuses { get; set; }
    }
}