namespace PizzaTracker.ViewModels
{
    public class EmailVm
    {
        public EmailVm()
        {
            to = "";
            user = "";
            link = "";
            message = "";
        }
        public string to { get; set; }
        public string user { get; set; }
        public string link { get; set; }
        public string message { get; set; }
        public int orderid { get; set; }
    }
}