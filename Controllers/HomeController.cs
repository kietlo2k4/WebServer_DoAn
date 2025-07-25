using Microsoft.AspNetCore.Mvc;

namespace WebServer_DoAn.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
