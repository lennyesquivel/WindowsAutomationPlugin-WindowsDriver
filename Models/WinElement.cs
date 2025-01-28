using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class WinElement
    {
        public By ByLocator { get; set; }
        public string LocatorValue { get; set; }

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
    }
}
