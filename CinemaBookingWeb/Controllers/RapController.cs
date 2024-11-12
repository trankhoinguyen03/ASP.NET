using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var movies = _context.Movies.ToList();
            var cities = _context.Cinemas.Select(c => c.City).Distinct().ToList();
            ViewBag.Cities = cities;
            return View(movies);
        }

        [HttpGet]
        public JsonResult GetCinemasByCity(string city)
        {
            var cinemas = _context.Cinemas
                .Where(c => c.City == city)
                .Select(c => new { c.CinemaId, c.Name, c.Location })
                .ToList();
            return Json(cinemas);
        }
    }
}
