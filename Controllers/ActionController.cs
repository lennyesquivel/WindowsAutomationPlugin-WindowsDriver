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
                return BadRequest(ModelState);
            }
            ResponseLog actionResult = null;
            switch(actionRequest.Action)
            {
                case Actions.Launch:
                    actionResult = _executionEngine.Launch(actionRequest.ActionValue);
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
                default:
                    actionResult = new ResponseLog(Responses.ActionNotImplemented);
                    break;

            }
            return CreatedAtAction("ExecuteAction", actionResult);
        }

        private WinElement buildWinElement(ActionRequest actionRequest)
        {
            return new WinElement(actionRequest.By, actionRequest.LocatorValue);
        }
    }
}
