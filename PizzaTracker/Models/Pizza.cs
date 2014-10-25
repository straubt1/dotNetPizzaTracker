using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class Pizza
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }

        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
        public int CrustId { get; set; }
        [ForeignKey("CrustId")]
        public virtual Crust Crust { get; set; }
        public virtual List<ToppingOption> Toppings { get; set; }
        //public int SauceId { get; set; }
        //[ForeignKey("SauceId")]
        public virtual SauceOption Sauce { get; set; }
    }
}