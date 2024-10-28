using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingWeb.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult NewUsers()
        {
            return View();
        }
        public IActionResult TotalTickets()
        {
            return View();
        }
        public IActionResult MoviesRevenue()
        {
            return View();
        }
        public IActionResult CinemasRevenue()
        {
            return View();
        }
        public IActionResult DailyRevenue()
        {
            return View();
        }
        public IActionResult MonthlyRevenue()
        {
            return View();
        }
    }
}
