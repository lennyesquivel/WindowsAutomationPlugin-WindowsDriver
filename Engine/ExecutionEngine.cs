using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using WindowsAutomationPlugin.Models;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Engine
{
    public class ExecutionEngine
    {
        private UIA3Automation _automation = new UIA3Automation();
        private Application? _runningApp;
        private FlaUI.Core.AutomationElements.Window? _mainWindow;
        private FlaUI.Core.AutomationElements.Window? _activeWindow;
        private ConditionFactory _conditionFactory = new ConditionFactory(new UIA3PropertyLibrary());
        public ExecutionEngine() { }

        public ResponseLog Launch(string path)
        {
            _runningApp = Application.Launch(path);
            _mainWindow = _runningApp.GetMainWindow(_automation);
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog AttachToProgram(string path)
        {
            _runningApp = Application.Attach(path);
            _mainWindow = _runningApp.GetMainWindow(_automation);
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog Click()
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.Click();
            return new ResponseLog();
        }

        public ResponseLog DoubleClick()
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.DoubleClick();
            return new ResponseLog();
        }

        public ResponseLog RightClick()
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Mouse.RightClick();
            return new ResponseLog();
        }

        public ResponseLog ClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            findElement(element).Click();
            return new ResponseLog();
        }

        public ResponseLog DoubleClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            findElement(element).DoubleClick();
            return new ResponseLog();
        }

        public ResponseLog RightClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            findElement(element).RightClick();
            return new ResponseLog();
        }

        public ResponseLog RightDoubleClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            findElement(element).RightDoubleClick();
            return new ResponseLog();
        }

        public ResponseLog Type(string value)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            Keyboard.Type(value);
            return new ResponseLog();
        }

        public ResponseLog TypeOnTextBox(WinElement element, string value)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            findElement(element).AsTextBox().Enter(value);
            return new ResponseLog();
        }

        public ResponseLog Close()
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            _runningApp.Close();
            _mainWindow = null;
            _runningApp = null;
            return new ResponseLog();
        }

        // TO-DO Implement screenshot
        public ResponseLog TakeScreenshot()
        {
            return new ResponseLog();
        }

        public AutomationElement? findElement(WinElement winElement)
        {
            switch (winElement.ByLocator)
            {
                case By.Name:
                    return _activeWindow.FindFirstNested(_conditionFactory.ByName(winElement.LocatorValue));
                case By.ClassName:
                    return _activeWindow.FindFirstNested(_conditionFactory.ByClassName(winElement.LocatorValue));
                case By.AutomationId:
                    return _activeWindow.FindFirstByXPath(winElement.LocatorValue);
                default: return null;
            }
        }
    }
}
