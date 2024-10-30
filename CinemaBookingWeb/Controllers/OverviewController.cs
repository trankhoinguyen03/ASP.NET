using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingWeb.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult NewUsers(DateTime? startDate, DateTime? endDate)
        {
            // Đặt ngày bắt đầu và kết thúc mặc định nếu không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            // Thống kê theo ngày
            var newUsersByDay = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate)
                .GroupBy(u => u.SignupDate)
                .Select(g => new DayStatistic
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Lấy dữ liệu từ database và chuyển sang client để xử lý GroupBy theo tháng
            var newUsersByMonth = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate)
                .AsEnumerable() // Chuyển truy vấn sang client-side
                .GroupBy(u => new { u.SignupDate.Year, u.SignupDate.Month })
                .Select(g => new MonthStatistic
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToList();

            var model = new OverviewModel
            {
                NewUsersByDay = newUsersByDay,
                NewUsersByMonth = newUsersByMonth
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
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
        public IActionResult TotalRevenue()
        {
            return View();
        }
    }
}
