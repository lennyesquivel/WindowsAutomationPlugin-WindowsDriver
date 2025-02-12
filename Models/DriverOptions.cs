namespace WindowsAutomationPlugin.Models
{
    public class DriverOptions
    {

        public int UIAVersion;
        public int ImplicitWaitTime;

        public String ToString()
        {
            return "UIAVersion: " + UIAVersion + " - ImplicitWaitTime: " + ImplicitWaitTime;
        }

    }
}
