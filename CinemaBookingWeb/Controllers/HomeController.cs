using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Data;
using Microsoft.EntityFrameworkCore;
using CinemaBookingWeb.ViewModels;

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
            var mainBanners = _context.Banner.Where(b => b.Category == "Main").ToList();
            var movies = _context.Movies.ToList();

            var viewModel = new HomePageViewModel
            {
                MainBanners = mainBanners,
                Movies = movies
            };

            return View(viewModel);
        }


    }
}
