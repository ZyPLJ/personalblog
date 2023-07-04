using Microsoft.AspNetCore.Mvc;

namespace Personalblog.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
