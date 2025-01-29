using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class ResponseLog
    {
        private Response _response { get; set; }
        private WinElement _element { get; set; }

        public ResponseLog()
        {
            _response = Responses.Success;
        }

        public ResponseLog(Response initResponse)
        {
            _response = initResponse;
        }

        public ResponseLog setElement(WinElement element)
        {
            _element = element;
            return this;
        }

        public WinElement getElement()
        {
            return _element;
        }
    }
}
