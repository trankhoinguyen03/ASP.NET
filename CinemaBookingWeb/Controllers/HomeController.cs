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
            var movies = _context.Movies.ToList(); // Lấy danh sách phim
            var banners = _context.Banner.ToList(); // Lấy danh sách banner

            var viewModel = new HomePageViewModel
            {
                Movies = movies,
                Banners = banners
            };

            return View(viewModel); // Truyền ViewModel vào view
        }

    }
}
