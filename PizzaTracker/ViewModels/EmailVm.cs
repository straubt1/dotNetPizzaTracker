namespace PizzaTracker.ViewModels
{
    public class EmailVm
    {
        public EmailVm()
        {
            to = "";
            subject = "";
            message = "";
        }
        public string to { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}