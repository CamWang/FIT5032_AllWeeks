using Microsoft.AspNetCore.Mvc;
using FIT5032_W3.HelloWorld;

namespace FIT5032_W3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = new Hello().GetHello();
            return View();
        }
    }
}
