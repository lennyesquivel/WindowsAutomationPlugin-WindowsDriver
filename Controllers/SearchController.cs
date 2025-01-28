using Microsoft.AspNetCore.Mvc;

namespace WindowsAutomationPlugin.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
