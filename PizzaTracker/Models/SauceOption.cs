using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class SauceOption
    {
        public virtual int Id { get; set; }
        public int SauceId { get; set; }
        [ForeignKey("SauceId")]
        public virtual Sauce Sauce { get; set; }
        public int SauceLevelId { get; set; }
        [ForeignKey("SauceLevelId")]
        public virtual SauceLevel SauceLevel { get; set; }
    }
}