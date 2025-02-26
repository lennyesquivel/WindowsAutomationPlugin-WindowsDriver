using System.Text.Json.Nodes;

namespace WindowsAutomationPlugin.Models
{
    public class ClientSession
    {
        public string ClientSessionId;
        public ClientSession() { }
        public ClientSession(JsonObject json)
        {
            ClientSession session = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientSession>(json.ToJsonString());
            this.ClientSessionId = session.ClientSessionId;
        }
    }
}
