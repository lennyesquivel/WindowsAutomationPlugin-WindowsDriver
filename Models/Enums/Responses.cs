namespace WindowsAutomationPlugin.Models.Enums
{
    public class Responses
    {
        // SUCCESS
        public static Response Success = new Response("SUC001", 200, "Success");
        // INFORMATION

        // ERRORS   
        public static Response InternalError = new Response("ERR001", 500, "Internal Error");
        public static Response WindowNotFound = new Response("WNF001", 404, "Window element not attached, launch first or attach to running program");
        public static Response ActionNotImplemented = new Response("ANI001", 501, "Action not implemented");
        public static Response BadRequest = new Response("REQ001", 502, "Bad Request");
        public static Response ActionError = new Response("ACT001", 503, "There was an error executing this action");

        // AUTH
        public static Response SessionNotRegistered = new Response("SNR001", 501, "Client Session not registered");
        public static Response ClientIdMismatch = new Response("MSM001", 503, "Provided Session Id doesn't match registered session's Id");
    }
}
