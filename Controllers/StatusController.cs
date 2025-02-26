using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using WindowsAutomationPlugin.Models;

namespace WindowsAutomationPlugin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly ILogger<StatusController> _logger;
        private IHostApplicationLifetime _lifeTime;
        // TO-DO cache the serverstatus obj
        public ServerStatus status = new ServerStatus(200, "Ready. Listening...");
        public string ClientSessionId = null;

        public StatusController(ILogger<StatusController> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _lifeTime = appLifetime;
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

        [HttpPost("clientSessionId")]
        public ServerStatus Post([FromBody] JsonObject clientSessionJson)
        {
            ClientSession clientSession = new ClientSession(clientSessionJson);
            if (HttpContext.Session.GetString("clientSessionId") != null)
            {
                return new ServerStatus(300, "There is a client already connected to this session.");
            }
            HttpContext.Session.SetString("clientSessionId", clientSession.ClientSessionId);
            this.ClientSessionId = clientSession.ClientSessionId;
            this.status = new ServerStatus(200, "Client session logged with sessionId: " + this.ClientSessionId);
            return status;
        }

        [HttpDelete("clientSessionId")]
        public ServerStatus DeleteSessionId()
        {
            HttpContext.Session.Remove("clientSessionId");
            return new ServerStatus(200, "Deleted current client session.");
        }

        [HttpDelete("selfDestruct")]
        public void SelfDestruct()
        {
            _lifeTime.StopApplication();
        }

    }
}
