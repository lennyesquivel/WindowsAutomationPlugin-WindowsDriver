using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class ActionRequest
    {
        public Actions Action;
        public string ActionValue;
        public By By;
        public string LocatorValue;
        public ActionRequest() { }
    }
}
