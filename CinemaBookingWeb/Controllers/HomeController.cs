using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Data;
using Microsoft.EntityFrameworkCore;


namespace CinemaBookingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList(); // Lấy danh sách các phim từ database
            return View(movies); // Truyền danh sách vào view
        }
        
    }
}
