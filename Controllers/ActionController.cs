using System.Text.Json.Nodes;
using FlaUI.Core.AutomationElements;
using Microsoft.AspNetCore.Mvc;
using WindowsAutomationPlugin.Engine;
using WindowsAutomationPlugin.Models;
using WindowsAutomationPlugin.Models.Enums;

namespace WindowsAutomationPlugin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;
        private static ExecutionEngine cache;
        private static object cacheLock = new object();
        private static ExecutionEngine _executionEngine
        {
            get
            {
                lock (cacheLock)
                {
                    if (cache == null)
                    {
                        cache = new ExecutionEngine();
                    }
                    return cache;
                }
            }
        }

        public ActionController(ILogger<ActionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ResponseLog Post([FromBody] JsonObject requestBody)
        {
            if (!ModelState.IsValid || requestBody == null)
            {
                logMessage("Error", String.Format("Invalid Request.\n{0}", ModelState));
                return new ResponseLog(Responses.BadRequest);
            }
            Console.WriteLine(requestBody.ToJsonString());
            ActionRequest actionRequest = new(requestBody);
            return HandleActionPost(actionRequest);
        }

        public ResponseLog HandleActionPost(ActionRequest actionRequest)
        {
            ResponseLog? actionResult = null;
            logMessage("Info", String.Format("Received action request: {0}", actionRequest.ToString()));
            switch (actionRequest.Action)
            {
                case Actions.Launch:
                    actionResult = _executionEngine.Launch(actionRequest.ActionValue);
                    break;
                case Actions.LaunchStoreApp:
                    actionResult = _executionEngine.LaunchStoreApp(actionRequest.ActionValue);
                    break;
                case Actions.AttachToProgram:
                    actionResult = _executionEngine.AttachToProgram(actionRequest.ActionValue);
                    break;
                case Actions.Click:
                    actionResult = _executionEngine.Click();
                    break;
                case Actions.DoubleClick:
                    actionResult = _executionEngine.DoubleClick();
                    break;
                case Actions.RightClick:
                    actionResult = _executionEngine.RightClick();
                    break;
                case Actions.ClickOnElement:
                    actionResult = _executionEngine.ClickOnElement(buildWinElement(actionRequest));
                    break;
                case Actions.RightClickOnElement:
                    actionResult = _executionEngine.RightClickOnElement(buildWinElement(actionRequest));
                    break;
                case Actions.RightDoubleClickOnElement:
                    actionResult = _executionEngine.RightDoubleClickOnElement(buildWinElement(actionRequest));
                    break;
                case Actions.Type:
                    actionResult = _executionEngine.Type(actionRequest.ActionValue);
                    break;
                case Actions.TypeOnTextBox:
                    actionResult = _executionEngine.TypeOnTextBox(buildWinElement(actionRequest), actionRequest.ActionValue);
                    break;
                case Actions.Close:
                    actionResult = _executionEngine.Close();
                    break;
                case Actions.TakeScreenshot:
                    actionResult = _executionEngine.TakeScreenshot();
                    break;
                case Actions.Wait:
                    actionResult = _executionEngine.Wait(int.Parse(actionRequest.ActionValue));
                    break;
                case Actions.Highlight:
                    actionResult = _executionEngine.Highlight(buildWinElement(actionRequest));
                    break;
                case Actions.TypeSimultaneously:
                    actionResult = _executionEngine.TypeSimultaneously(actionRequest.Keys);
                    break;
                default:
                    actionResult = new ResponseLog(Responses.ActionNotImplemented);
                    break;

            }
            return actionResult;
        }

        [HttpGet("element")]
        public WinElement GetElement(string locatorType, string locatorValue)
        {
            logMessage("Info", String.Format("Received get element request: {0}, {1}", locatorType, locatorValue));
            Enum.TryParse(locatorType, out By by);
            //TO-DO get native element properties and write to winelement class
            AutomationElement element = _executionEngine.FindElementByValues(by, locatorValue);
            return new WinElement(by, locatorValue, element);
        }

        [HttpGet("elements")]
        public List<WinElement> GetElements(ActionRequest actionRequest)
        {
            logMessage("Info", String.Format("Received get element request: {0}", actionRequest));
            return findElements(actionRequest);
        }

        private WinElement buildWinElement(ActionRequest actionRequest)
        {
            return new WinElement(actionRequest.By, actionRequest.LocatorValue);
        }

        private List<WinElement> findElements(ActionRequest actionRequest)
        {
            List<AutomationElement> elements = _executionEngine.FindElements(actionRequest.By, actionRequest.LocatorValue);
            List<WinElement> winElements = new List<WinElement>();
            foreach (AutomationElement element in elements)
            {
                //TO-DO get native element properties and write to winelement class
                winElements.Add(new WinElement(actionRequest));
            }
            return winElements;
        }

        private void logMessage(string type, string message)
        {
            if (_logger != null)
            {
                switch(type)
                {
                    case "Error":
                        _logger.LogError(message);
                        break;
                    case "Info":
                    default:
                        _logger.LogInformation(message);
                        break;
                }
            } else
            {
                switch (type)
                {
                    case "Error":
                        Console.WriteLine("Error: " + message);
                        break;
                    case "Info":
                    default:
                        Console.WriteLine("Info: " + message);
                        break;
                }
            }
        }
    }
}
