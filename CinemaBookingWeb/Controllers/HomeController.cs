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

        public void UpdateExpiredShowtimes()
        {
            var now = DateTime.Now;

            // Lấy danh sách các showtimes đã qua và trạng thái là "đang chiếu"
            var expiredShowtimes = _context.Showtimes
                .Where(b => b.StartTime < now && b.Status == 1) // Chỉ áp dụng cho trạng thái "Đang chiếu"
                .ToList();

            foreach (var showtime in expiredShowtimes)
            {
                showtime.Status = 0; // Chuyển trạng thái thành "Hủy"
            }

            _context.SaveChanges();
        }

        public IActionResult Index()
        {
            UpdateExpiredShowtimes(); // Cập nhật trạng thái các showtime hết hạn
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
