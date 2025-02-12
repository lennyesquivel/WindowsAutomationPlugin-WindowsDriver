using Microsoft.AspNetCore.Mvc;
using WindowsAutomationPlugin.Models;

namespace WindowsAutomationPlugin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly ILogger<StatusController> _logger;
        // TO-DO cache the serverstatus obj
        public ServerStatus status = new ServerStatus(200, "Ready. Listening...");
        public string ClientSessionId = null;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
            this.status = new ServerStatus(200, "Ready. Listening... " + (ClientSessionId != null ? "With client sessionId: " + ClientSessionId : "No client connected yet."));
        }

        public ServerStatus Get()
        {
            return status;
        }

        [HttpGet("clientSessionId")]
        public String GetClientSessionId()
        {
            return HttpContext.Session.GetString("clientSessionId");
        }

        [HttpPost]
        public ServerStatus Post(string clientSessionId)
        {
            HttpContext.Session.SetString("clientSessionId", clientSessionId);
            this.ClientSessionId = clientSessionId;
            this.status = new ServerStatus(200, "Client session logged with sessionId: " + this.ClientSessionId);
            return status;
        }

        [HttpDelete("clientSessionId")]
        public ServerStatus DeleteSessionId()
        {
            HttpContext.Session.Remove("clientSessionId");
            return new ServerStatus(200, "Deleted current client session.");
        }

    }
}
