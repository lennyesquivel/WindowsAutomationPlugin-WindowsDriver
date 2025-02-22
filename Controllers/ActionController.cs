using System.Text.Json.Nodes;
using FlaUI.Core.AutomationElements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        private ResponseLog checkClientSessionId(String sessionIdFromReq)
        {
            string clientSessionId = HttpContext.Session.GetString("clientSessionId");
            Console.WriteLine("Received request from clientSessionId: " + sessionIdFromReq);
            Console.WriteLine("Comparing against: " + clientSessionId);
            if (clientSessionId == null)
            {
                return new ResponseLog(Responses.SessionNotRegistered);
            }
            else if (clientSessionId != sessionIdFromReq)
            {
                return new ResponseLog(Responses.ClientIdMismatch);
            }
            return null;
        }

        [HttpPost("driver")]
        public ResponseLog SetupDriver([FromBody] DriverOptions options)
        {
            Request.Headers.TryGetValue("clientSessionId", out StringValues clientId);
            ResponseLog clientSessionRes = checkClientSessionId(clientId);
            if (clientSessionRes != null)
            {
                return clientSessionRes;
            } 
            _executionEngine.setImplicitWaitTime(options.ImplicitWaitTime);
            Console.WriteLine(options.ToString());
            return new ResponseLog(Responses.Success);
        }

        [HttpPost]
        public ResponseLog Post([FromBody] JsonObject requestBody)
        {
            if (!ModelState.IsValid || requestBody == null)
            {
                logMessage("Error", String.Format("Invalid Request.\n{0}", ModelState));
                return new ResponseLog(Responses.BadRequest);
            }
            Request.Headers.TryGetValue("clientSessionId", out StringValues clientId);
            ResponseLog clientSessionRes = checkClientSessionId(clientId);
            if (clientSessionRes != null)
            {
                return clientSessionRes;
            }
            string clientSessionId = HttpContext.Session.GetString("clientSessionId");
            Console.WriteLine("Registered Client Session ID: " + clientSessionId);
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
                case Actions.MoveMouseToPosition:
                    string coords = actionRequest.ActionValue.Substring(1, actionRequest.ActionValue.Length - 2);
                    int X = int.Parse(coords.Split(",")[0]);
                    int Y = int.Parse(coords.Split(",")[1]);
                    actionResult = _executionEngine.MoveMouseToPosition(X, Y);
                    break;
                case Actions.KeyDown:
                    Enum.TryParse(actionRequest.ActionValue, out FlaUI.Core.WindowsAPI.VirtualKeyShort keyDown);
                    actionResult = _executionEngine.KeyDown(keyDown);
                    break;
                case Actions.KeyUp:
                    Enum.TryParse(actionRequest.ActionValue, out FlaUI.Core.WindowsAPI.VirtualKeyShort keyUp);
                    actionResult = _executionEngine.KeyUp(keyUp);
                    break;
                case Actions.ClickAndDragToCoordinates:
                    string dragDropCoords = actionRequest.ActionValue.Substring(1, actionRequest.ActionValue.Length - 2);
                    int dX = int.Parse(dragDropCoords.Split(",")[0]);
                    int dY = int.Parse(dragDropCoords.Split(",")[1]);
                    actionResult = _executionEngine.ClickAndDragToCoordinates(dX, dY);
                    break;
                case Actions.ClickAndDragToElement:
                    actionResult = _executionEngine.ClickAndDragToElement(buildWinElement(actionRequest));
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
            Request.Headers.TryGetValue("clientSessionId", out StringValues clientId);
            ResponseLog clientSessionRes = checkClientSessionId(clientId);
            if (clientSessionRes != null)
            {
                return null;
            }
            logMessage("Info", String.Format("Received get element request: {0}, {1}", locatorType, locatorValue));
            Enum.TryParse(locatorType, out By by);
            AutomationElement element = _executionEngine.FindElementByValues(by, locatorValue);
            if (element == null)
            {
                return null;
            }
            return new WinElement(by, locatorValue, element);
        }

        [HttpGet("elements")]
        public List<WinElement> GetElements(ActionRequest actionRequest)
        {
            Request.Headers.TryGetValue("clientSessionId", out StringValues clientId);
            ResponseLog clientSessionRes = checkClientSessionId(clientId);
            if (clientSessionRes != null)
            {
                return null;
            }
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
