using FlaUI.Core.AutomationElements;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class WinElement
    {
        public By ByLocator { get; set; }
        public string LocatorValue { get; set; }
        public Object NativeElement { get; set; }

        public WinElement()
        {
            ByLocator = By.AutomationId;
            LocatorValue = "";
        }

        public WinElement(By initBy, string initLocatorValue)
        {
            this.ByLocator = initBy;
            this.LocatorValue = initLocatorValue;
        }

        public WinElement(ActionRequest actionRequest, AutomationElement element)
        {
            this.ByLocator = actionRequest.By;
            this.LocatorValue = actionRequest.LocatorValue;
            this.NativeElement = element;
        }
    }
}
