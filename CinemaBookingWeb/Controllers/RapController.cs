using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingWeb.Controllers
{
    public class RapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RapController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Rap_Giave()
        {
            var movies = _context.Movies.ToList(); // Lấy danh sách các phim từ database
            return View(movies); // Truyền danh sách vào view
        }
    }
}
