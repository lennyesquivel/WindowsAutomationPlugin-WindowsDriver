﻿using FlaUI.Core.AutomationElements;
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
        }

    }
}
