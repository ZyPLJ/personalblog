using Microsoft.AspNetCore.Mvc;

namespace Personalblog.Controllers;

public class ToolController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}