namespace WindowsAutomationPlugin.Models
{
    public class Response
    {
        public string Code { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Response(string code, int statusCode, string message)
        {
            Code = code;
            StatusCode = statusCode;
            Message = message;
        }
        public string toString()
        {
            return String.Format("[{0}]: {1}. Status code: {2}", Code, Message, StatusCode);
        }
    }
}
