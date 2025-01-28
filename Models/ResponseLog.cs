using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class ResponseLog
    {
        private Response _response { get; set; }

        public ResponseLog()
        {
            _response = Responses.Success;
        }

        public ResponseLog(Response initResponse)
        {
            _response = initResponse;
        }
    }
}
