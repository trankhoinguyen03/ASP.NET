using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingWeb.Controllers
{
    public class MoviesDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int Id)
        {
            var movies = _context.Movies.Where(n => n.MovieId == Id).FirstOrDefault();
            return View(movies);
        }
    }
}
