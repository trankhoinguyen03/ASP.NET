using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
