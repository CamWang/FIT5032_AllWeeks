using Microsoft.AspNetCore.Mvc;

namespace StudioAssessment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToPage("/Index", new { Message = "Welcome to Ultrasound Appointment System" });
        }
    }
}
