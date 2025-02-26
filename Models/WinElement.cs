using FlaUI.Core.AutomationElements;
using FlaUI.UIA3.Converters;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Models
{
    public class WinElement
    {
        public By ByLocator { get; set; }
        public string LocatorValue { get; set; }
        public string Name { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string ClassName { get; set; }
        public string ControlType { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsOffscreen { get; set; }

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

        public WinElement(ActionRequest actionRequest)
        {
            this.ByLocator = actionRequest.By;
            this.LocatorValue = actionRequest.LocatorValue;
        }

        public WinElement(By initBy, string initLocatorValue, AutomationElement nativeElement)
        {
            this.ByLocator = initBy;
            this.LocatorValue = initLocatorValue;
            this.Name = nativeElement.Name;
            this.PositionX = nativeElement.BoundingRectangle.X + (nativeElement.BoundingRectangle.Width / 2);
            this.PositionY = nativeElement.BoundingRectangle.Y + (nativeElement.BoundingRectangle.Height / 2);
            this.Height = nativeElement.ActualHeight;
            this.Width = nativeElement.ActualWidth;
            try
            {
                this.ClassName = nativeElement.ClassName;
            }
            catch (Exception) { }
            this.ControlType = nativeElement.ControlType.ToString();
            this.IsAvailable = nativeElement.IsAvailable;
            this.IsEnabled = nativeElement.IsEnabled;
            this.IsOffscreen = nativeElement.IsOffscreen;
        }

    }
}
