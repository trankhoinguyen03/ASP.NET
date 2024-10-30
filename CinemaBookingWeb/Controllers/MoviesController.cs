using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MoviesDangChieu()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

    }
}
