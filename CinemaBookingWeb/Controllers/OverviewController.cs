﻿using CinemaBookingWeb.Data;
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

            // Số người dùng mới theo ngày
            var newUsersByDay = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate && u.Status == 1)
                .GroupBy(u => u.SignupDate)
                .Select(g => new DayStatistic_Count
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Số người dùng mới theo tháng
            var newUsersByMonth = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate && u.Status == 1)
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
        public IActionResult TotalTickets(DateTime? startDate, DateTime? endDate)
        {
            // Đặt ngày bắt đầu và kết thúc mặc định nếu không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            // Số vé bán ra theo ngày
            var totalTicketsByDay = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .Join(_context.BookingsDetails, b => b.BookingId, s => s.BookingId, (b, s) => new { b, s })
                .GroupBy(bs => bs.b.BookingDate)
                .Select(g => new DayStatistic_Count
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Số vé bán ra theo tháng
            var totalTicketsByMonth = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .Join(_context.BookingsDetails, b => b.BookingId, s => s.BookingId, (b, s) => new { b, s })
                .AsEnumerable()
                .GroupBy(bs => new { bs.b.BookingDate.Year, bs.b.BookingDate.Month })
                .Select(g => new MonthStatistic_Count
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToList();

            // Tạo model và truyền các thống kê vào view
            var model = new OverviewModel
            {
                TotalTicketsByDay = totalTicketsByDay,
                TotalTicketsByMonth = totalTicketsByMonth
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
        }
        public IActionResult MoviesRevenue(DateTime? startDate, DateTime? endDate)
        {
            // Đặt ngày bắt đầu và kết thúc mặc định nếu không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            // Thống kê doanh thu theo ngày cho từng bộ phim
            var moviesRevenueByDay = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s })
                .GroupBy(bs => bs.s.MovieId)
                .Select(g => new DayStatistic_Movies
                {
                    Name = _context.Movies.FirstOrDefault(m => m.MovieId == g.Key).Title,
                    Revenue = g.Sum(bs => bs.b.TotalPrice)
                })
                .OrderByDescending(g => g.Revenue)
                .ToList();

            // Thống kê doanh thu theo tháng cho từng bộ phim
            var moviesRevenueByMonth = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s })
                .AsEnumerable()
                .GroupBy(bs => bs.s.MovieId)
                .Select(g => new MonthStatistic_Movies
                {
                    Name = _context.Movies.FirstOrDefault(m => m.MovieId == g.Key).Title,
                    Revenue = g.Sum(bs => bs.b.TotalPrice)
                })
                .OrderByDescending(g => g.Revenue)
                .ToList();

            // Tạo model và truyền các thống kê vào view
            var model = new OverviewModel
            {
                MoviesRevenueByDay = moviesRevenueByDay,
                MoviesRevenueByMonth = moviesRevenueByMonth
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
        }
        //public IActionResult CinemasRevenue(DateTime? startDate, DateTime? endDate)
        //{
        //    // Đặt ngày bắt đầu và kết thúc mặc định nếu không được truyền vào
        //    startDate ??= DateTime.Now.AddMonths(-1);
        //    endDate ??= DateTime.Now;

        //    // Thống kê doanh thu theo ngày cho từng rạp
        //    var cinemasRevenueByDay = _context.Bookings
        //        .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
        //        .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s })
        //        .GroupBy(bs => bs.s.CinemaId)
        //        .Select(g => new DayStatistic_Movies
        //        {
        //            Name = _context.Cinemas.FirstOrDefault(m => m.CinemaId == g.Key).Name,
        //            Revenue = g.Sum(bs => bs.b.TotalPrice)
        //        })
        //        .OrderByDescending(g => g.Revenue)
        //        .ToList();

        //    // Thống kê doanh thu theo tháng cho từng rạp
        //    var cinemasRevenueByMonth = _context.Bookings
        //        .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
        //        .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s })
        //        .AsEnumerable()
        //        .GroupBy(bs => bs.s.CinemaId)
        //        .Select(g => new MonthStatistic_Movies
        //        {
        //            Name = _context.Cinemas.FirstOrDefault(m => m.CinemaId == g.Key).Name,
        //            Revenue = g.Sum(bs => bs.b.TotalPrice)
        //        })
        //        .OrderByDescending(g => g.Revenue)
        //        .ToList();

        //    // Tạo model và truyền các thống kê vào view
        //    var model = new OverviewModel
        //    {
        //        CinemasRevenueByDay = cinemasRevenueByDay,
        //        CinemasRevenueByMonth = cinemasRevenueByMonth
        //    };

        //    ViewBag.StartDate = startDate;
        //    ViewBag.EndDate = endDate;

        //    return View(model);
        //}
        public IActionResult TotalRevenue(DateTime? startDate, DateTime? endDate)
        {
            // Đặt giá trị mặc định cho ngày bắt đầu và ngày kết thúc nếu chúng không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-1);
            endDate ??= DateTime.Now;

            // Tính tổng doanh thu theo ngày
            var totalRevenueByDay = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
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
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
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
