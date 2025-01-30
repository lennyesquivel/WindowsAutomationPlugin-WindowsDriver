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
        private readonly ExecutionEngine _executionEngine;

        public ActionController(ILogger<ActionController> logger)
        {
            _logger = logger;
            _executionEngine = new ExecutionEngine();
        }

        [HttpPost]
        public IActionResult Post([FromBody] ActionRequest actionRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Request.\n{0}", ModelState);
                return BadRequest(ModelState);
            }
            ResponseLog actionResult = null;
            _logger.LogInformation("Received action request: {0}", actionRequest);
            switch(actionRequest.Action)
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
                    _executionEngine.TakeScreenshot();
                    actionResult = new ResponseLog(Responses.ActionNotImplemented);
                    break;
                case Actions.Wait:
                    actionResult = _executionEngine.Wait(int.Parse(actionRequest.ActionValue));
                    break;
                case Actions.Highlight:
                    actionResult = _executionEngine.Highlight(buildWinElement(actionRequest));
                    break;
                default:
                    actionResult = new ResponseLog(Responses.ActionNotImplemented);
                    break;

            }
            return CreatedAtAction("ExecuteAction", actionResult);
        }

        [HttpGet("element")]
        public IEnumerable<WinElement> GetElement(string locatorType, string locatorValue)
        {
            _logger.LogInformation("Received get element request: {0}, {1}", locatorType, locatorValue);
            Enum.TryParse(locatorType, out By by);
            yield return new WinElement(by, locatorValue);
        }

        [HttpGet("elements")]
        public List<WinElement> GetElements(ActionRequest actionRequest)
        {
            _logger.LogInformation("Received get element request: {0}", actionRequest);
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
                winElements.Add(new WinElement(actionRequest, element));
            }
            return winElements;
        }
    }
}
