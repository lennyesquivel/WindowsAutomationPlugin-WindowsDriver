using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class ResponseLog
    {
        private Response _response { get; set; }
        private WinElement _element { get; set; }
        private Object _data { get; set; }

        public ResponseLog()
        {
            _response = Responses.Success;
        }

        public ResponseLog(Response initResponse)
        {
            _response = initResponse;
        }

        public ResponseLog SetElement(WinElement element)
        {
            _element = element;
            return this;
        }

        public WinElement GetElement()
        {
            return _element;
        }

        public ResponseLog SetData(Object setData)
        {
            _data = setData;
            return this;
        }
    }
}
