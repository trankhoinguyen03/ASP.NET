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
                .Select(g => new DayStatistic_Count
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
                .Select(g => new MonthStatistic_Count
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
        public IActionResult TotalRevenue(DateTime? startDate, DateTime? endDate)
        {
            // Đặt giá trị mặc định cho ngày bắt đầu và ngày kết thúc nếu chúng không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            // Tính tổng doanh thu theo ngày
            var totalRevenueByDay = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new DayStatistic_Revenue
                {
                    Date = g.Key,
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Tính tổng doanh thu theo tháng
            var totalRevenueByMonth = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .AsEnumerable() // Switch to client-side grouping
                .GroupBy(b => new { b.BookingDate.Year, b.BookingDate.Month })
                .Select(g => new MonthStatistic_Revenue
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(g => g.Month)
                .ToList();

            // Tạo model và truyền các thống kê vào view
            var model = new OverviewModel
            {
                TotalRevenueByDay = totalRevenueByDay,
                TotalRevenueByMonth = totalRevenueByMonth
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
        }
    }
}
