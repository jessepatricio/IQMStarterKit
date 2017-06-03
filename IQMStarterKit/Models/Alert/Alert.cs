namespace IQMStarterKit.Models.Alert
{
    public class Alert

    {

        public string Command { get; set; }

        public string Message { get; set; }



        public Alert(string command, string message)

        {

            Command = command;

            Message = message;

        }

    }
}
