using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeAdminController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeAdminController(ILogger<HomeAdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
