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
    }
}
