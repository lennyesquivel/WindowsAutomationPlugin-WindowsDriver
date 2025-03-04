﻿using System.Configuration;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using WindowsAutomationPlugin.Models;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Engine
{
    public class ExecutionEngine
    {

        //private ILogger<ExecutionEngine> _logger = new();
        private FlaUI.Core.Application? _runningApp;
        private FlaUI.Core.AutomationElements.Window? _mainWindow;
        private FlaUI.Core.AutomationElements.Window? _activeWindow;
        private readonly ConditionFactory _conditionFactory = new(new UIA3PropertyLibrary());
        private int implicitWaitMilis = 500;

        public ExecutionEngine() { }

        public ResponseLog Launch(string path)
        {
            //_logger.LogInformation("Launching app: " + path);
            _runningApp = FlaUI.Core.Application.Launch(path);
            UIA3Automation _automation = new();
            _mainWindow = Retry.WhileNull(() => _runningApp.GetMainWindow(_automation), TimeSpan.FromSeconds(10)).Result;
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog LaunchStoreApp(string aumid)
        {
            //_logger.LogInformation("Launching StoreApp: " +  aumid);
            _runningApp = FlaUI.Core.Application.LaunchStoreApp(aumid);
            UIA3Automation _automation = new();
            _mainWindow = Retry.WhileNull(() => _runningApp.GetMainWindow(_automation), TimeSpan.FromSeconds(10)).Result;
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog AttachToProgram(string path)
        {
            //_logger.LogInformation("Attaching to running program: " + path);
            _runningApp = FlaUI.Core.Application.Attach(path);
            UIA3Automation _automation = new();
            _mainWindow = _runningApp.GetMainWindow(_automation);
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog Click()
        {
            //_logger.LogInformation("Performing click.");
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.Click();
            return new ResponseLog();
        }

        public ResponseLog DoubleClick()
        {
            //_logger.LogInformation("Performing double click.");
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.DoubleClick();
            return new ResponseLog();
        }

        public ResponseLog RightClick()
        {
            //_logger.LogInformation("Performing right click");
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.RightClick();
            return new ResponseLog();
        }

        public ResponseLog ClickOnElement(WinElement element)
        {
            //_logger.LogInformation("Performing click on element: " + element.ToString());
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).Click();
            return new ResponseLog();
        }

        public ResponseLog DoubleClickOnElement(WinElement element)
        {
            //_logger.LogInformation("Performing double click on element: " + element.ToString());
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).DoubleClick();
            return new ResponseLog();
        }

        public ResponseLog RightClickOnElement(WinElement element)
        {
            //_logger.LogInformation("Performing right click on element: " + element.ToString());
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).RightClick();
            return new ResponseLog();
        }

        public ResponseLog RightDoubleClickOnElement(WinElement element)
        {
            //_logger.LogInformation("Performing right double click on element: " + element.ToString());
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).RightDoubleClick();
            return new ResponseLog();
        }

        public ResponseLog Type(string value)
        {
            //_logger.LogInformation("Performing type with value: " + value);
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Keyboard.Type(value);
            return new ResponseLog();
        }

        public ResponseLog TypeOnTextBox(WinElement element, string value)
        {
            //_logger.LogInformation("Performing type on textbox: " + element.ToString());
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).AsTextBox().Enter(value);
            return new ResponseLog();
        }

        public ResponseLog Close()
        {
            //_logger.LogInformation("Closing running application.");
            if (_activeWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            _runningApp.Close();
            _activeWindow = null;
            _mainWindow = null;
            _runningApp = null;
            return new ResponseLog();
        }

        public ResponseLog TakeScreenshot()
        {
            //_logger.LogInformation("Capturing screenshot.");
            CaptureImage capture = Capture.Screen();
            return new ResponseLog().SetData(setData: capture);
        }

        public ResponseLog Wait(int seconds)
        {
            //_logger.LogInformation("Waiting for seconds: " + seconds);
            Thread.Sleep(seconds * 1000);
            return new ResponseLog();
        }

        public ResponseLog GetElement(WinElement element)
        {
            return new ResponseLog().SetElement(element);
        }

        public ResponseLog Highlight(WinElement element)
        {
            FindElement(element).DrawHighlight();
            return new ResponseLog();
        }

        public ResponseLog TypeSimultaneously(VirtualKeyShort[] keys)
        {
            Keyboard.TypeSimultaneously(keys);
            return new ResponseLog();
        }

        public ResponseLog MoveMouseToPosition(int X, int Y)
        {
            Mouse.MoveTo(X, Y);
            return new ResponseLog();
        }

        public ResponseLog KeyDown(VirtualKeyShort key)
        {
            Keyboard.PressVirtualKeyCode((ushort)key);
            return new ResponseLog();
        }

        public ResponseLog KeyUp(VirtualKeyShort key)
        {
            Keyboard.ReleaseVirtualKeyCode((ushort)key);
            return new ResponseLog();
        }

        public ResponseLog ClickAndDragToCoordinates(int x, int y)
        {
            Mouse.Down();
            Mouse.MoveTo(x, y);
            Mouse.Up();
            return new ResponseLog();
        }

        public ResponseLog ClickAndDragToElement(WinElement element)
        {
            AutomationElement autEl = FindElement(element);
            if (autEl != null)
            {
                int X = autEl.BoundingRectangle.X + (autEl.BoundingRectangle.Width / 2);
                int Y = autEl.BoundingRectangle.Y + (autEl.BoundingRectangle.Height / 2);
                return ClickAndDragToCoordinates(X, Y);
            }
            return new ResponseLog(Responses.ActionError);
        }

        public AutomationElement? FindElement(WinElement winElement)
        {
            return FindElementByValues(winElement.ByLocator, winElement.LocatorValue);
        }

        public AutomationElement? FindElementByValues(By by, string locatorValue)
        {
            switch (by)
            {
                case By.Name:
                    return WaitForElement(() => _activeWindow.FindFirstDescendant(_conditionFactory.ByName(locatorValue)));
                case By.ClassName:
                    return WaitForElement(() => _activeWindow.FindFirstDescendant(_conditionFactory.ByClassName(locatorValue)));
                case By.AutomationId:
                    return WaitForElement(() => _activeWindow.FindFirstDescendant(_conditionFactory.ByAutomationId(locatorValue)));
                case By.Value:
                    return WaitForElement(() => _activeWindow.FindFirstDescendant(_conditionFactory.ByValue(locatorValue)));
                case By.Text:
                    return WaitForElement(() => _activeWindow.FindFirstDescendant(_conditionFactory.ByText(locatorValue)));
                case By.Xpath:
                    return _activeWindow.FindFirstByXPath(locatorValue);
                default: throw new Exception("Locating type not available or implemented");
            }
        }

        public List<AutomationElement> FindElements(By by, string locatorValue)
        {
            switch (by)
            {
                case By.Name:
                    return [.. _activeWindow.FindAllDescendants(_conditionFactory.ByName(locatorValue))];
                case By.ClassName:
                    return [.. _activeWindow.FindAllDescendants(_conditionFactory.ByClassName(locatorValue))];
                case By.AutomationId:
                    return [.. _activeWindow.FindAllDescendants(_conditionFactory.ByAutomationId(locatorValue))];
                case By.Value:
                    return [.. _activeWindow.FindAllDescendants(_conditionFactory.ByValue(locatorValue))];
                case By.Text:
                    return [.. _activeWindow.FindAllDescendants(_conditionFactory.ByText(locatorValue))];
                case By.Xpath:
                    return [.. _activeWindow.FindAllByXPath(locatorValue)];
                default: throw new Exception("Locating type not available or implemented");
            }
        }

        public void setImplicitWaitTime(int milis)
        {
            this.implicitWaitMilis = milis;
        }

        private T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull<T>(
                () => getter(),
                TimeSpan.FromMilliseconds(implicitWaitMilis));

            if (!retry.Success)
            {
                throw new Exception("Element could not be found");
            }

            return retry.Result;
        }

    }
}
