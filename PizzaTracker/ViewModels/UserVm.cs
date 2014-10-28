namespace PizzaTracker.ViewModels
{
    public class UserVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }

    public class LoginVm
    {
        public int UserId { get; set; }
        public string UserToken { get; set; }
    }
}