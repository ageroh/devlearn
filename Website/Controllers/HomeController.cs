using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Poll()
        {
            return View();
        }

        public IActionResult Push()
        {
            return View();
        }
    }
}
