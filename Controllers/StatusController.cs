using Microsoft.AspNetCore.Mvc;
using WindowsAutomationPlugin.Models;

namespace WindowsAutomationPlugin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly ILogger<StatusController> _logger;

        public ServerStatus status = new ServerStatus(200, "Ready.\nListening...");

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
            status = new ServerStatus(200, "Ready.\nListening...");
        }

        public ServerStatus Get()
        {
            return status;
        }
    }
}
