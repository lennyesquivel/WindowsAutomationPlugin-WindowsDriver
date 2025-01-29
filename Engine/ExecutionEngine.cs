using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using WindowsAutomationPlugin.Models;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Engine
{
    public class ExecutionEngine
    {
        private Application? _runningApp;
        private FlaUI.Core.AutomationElements.Window? _mainWindow;
        private FlaUI.Core.AutomationElements.Window? _activeWindow;
        private readonly ConditionFactory _conditionFactory = new(new UIA3PropertyLibrary());
        public ExecutionEngine() { }

        public ResponseLog Launch(string path)
        {
            _runningApp = Application.Launch(path);
            UIA3Automation _automation = new();
            _mainWindow = Retry.WhileNull(() => _runningApp.GetMainWindow(_automation), TimeSpan.FromSeconds(10)).Result;
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog LaunchStoreApp(string name)
        {
            _runningApp = Application.LaunchStoreApp(name);
            UIA3Automation _automation = new();
            _mainWindow = _runningApp.GetMainWindow(_automation);
            _activeWindow = _mainWindow;
            return new ResponseLog();
        }

        public ResponseLog AttachToProgram(string path)
        {
            _runningApp = Application.Attach(path);
            UIA3Automation _automation = new();
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
            FindElement(element).Click();
            return new ResponseLog();
        }

        public ResponseLog DoubleClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).DoubleClick();
            return new ResponseLog();
        }

        public ResponseLog RightClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).RightClick();
            return new ResponseLog();
        }

        public ResponseLog RightDoubleClickOnElement(WinElement element)
        {
            if (_mainWindow == null)
            {
                return new ResponseLog(Responses.WindowNotFound);
            }
            FindElement(element).RightDoubleClick();
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
            FindElement(element).AsTextBox().Enter(value);
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

        public ResponseLog Wait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            return new ResponseLog();
        }

        public ResponseLog GetElement(WinElement element)
        {
            element.NativeElement = FindElement(element);
            return new ResponseLog().setElement(element);
        }

        public ResponseLog Highlight(WinElement element)
        {
            FindElement(element).DrawHighlight();
            return new ResponseLog();
        }

        public AutomationElement? FindElement(WinElement winElement)
        {
            switch (winElement.ByLocator)
            {
                case By.Name:
                    return _activeWindow.FindFirstDescendant(_conditionFactory.ByName(winElement.LocatorValue));
                case By.ClassName:
                    return _activeWindow.FindFirstDescendant(_conditionFactory.ByClassName(winElement.LocatorValue));
                case By.AutomationId:
                    return _activeWindow.FindFirstDescendant(_conditionFactory.ByAutomationId(winElement.LocatorValue));
                case By.Value:
                    return _activeWindow.FindFirstDescendant(_conditionFactory.ByValue(winElement.LocatorValue));
                case By.Text:
                    return _activeWindow.FindFirstDescendant(_conditionFactory.ByText(winElement.LocatorValue));
                case By.Xpath:
                    return _activeWindow.FindFirstByXPath(winElement.LocatorValue);
                default: return null;
            }
        }
    }
}
