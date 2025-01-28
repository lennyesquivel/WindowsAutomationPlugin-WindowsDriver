namespace WindowsAutomationPlugin.Models
{
    public class ServerStatus
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public string Log { get; set; }

        public ServerStatus()
        {
            StatusCode = 200;
            StatusMessage = "Ready";
            Log = "";
        }

        public ServerStatus(int statusCode, string statusMessage)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Log = "";
        }

        public void update(int statusCode, string statusMessage)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Log = "";
        }

        public void update(int statusCode, string statusMessage, string log)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Log = log;
        }

    }
}
