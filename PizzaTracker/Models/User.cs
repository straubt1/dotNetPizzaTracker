using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaTracker.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string PasswordResetToken { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual string LoginToken { get; set; }
        public virtual DateTime? LoginExpiration { get; set; }
    }
}