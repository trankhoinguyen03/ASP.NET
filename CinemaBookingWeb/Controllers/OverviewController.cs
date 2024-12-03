using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
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
            startDate ??= DateTime.Now.AddMonths(-3);
            endDate ??= DateTime.Now;

            // Số người dùng mới theo ngày

            var newUsersByDay = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate && u.Role == "User" && u.Status == 1)
                .GroupBy(u => u.SignupDate.Date)
                .Select(g => new DayStatistic_Count
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Số người dùng mới theo tháng
            var newUsersByMonth = _context.Users
                .Where(u => u.SignupDate >= startDate && u.SignupDate <= endDate && u.Role == "User" && u.Status == 1)
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
            startDate ??= DateTime.Now.AddMonths(-3);
            endDate ??= DateTime.Now;

            // Số vé bán ra theo ngày
            var totalTicketsByDay = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
                .Join(_context.BookingDetails, b => b.BookingId, s => s.BookingId, (b, s) => new { b, s })
                .GroupBy(bs => bs.b.BookingDate.Date)
                .Select(g => new DayStatistic_Count
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            // Số vé bán ra theo tháng
            var totalTicketsByMonth = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
                .Join(_context.BookingDetails, b => b.BookingId, s => s.BookingId, (b, s) => new { b, s })
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
            startDate ??= DateTime.Now.AddMonths(-3);
            endDate ??= DateTime.Now;

            // Thống kê doanh thu cho từng bộ phim
            var moviesRevenue = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
                .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s.MovieId })
                .GroupBy(bs => bs.MovieId)
                .Select(g => new Statistic_Movies_Cinemas
                {
                    Name = _context.Movies.Where(m => m.MovieId == g.Key).Select(m => m.Title).FirstOrDefault(),
                    Revenue = g.Sum(bs => bs.b.TotalPrice)
                })
                .OrderByDescending(g => g.Revenue)
                .ToList();

            // Tạo model và truyền thống kê vào view
            var model = new OverviewModel
            {
                MoviesRevenue = moviesRevenue
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
        }
        public IActionResult CinemasRevenue(DateTime? startDate, DateTime? endDate)
        {
            // Đặt ngày bắt đầu và kết thúc mặc định nếu không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-3);
            endDate ??= DateTime.Now;

            // Thống kê doanh thu cho từng rạp
            var cinemasRevenue = _context.Bookings
                .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.Status == 1)
                .Join(_context.Showtimes, b => b.ShowtimeId, s => s.ShowtimeId, (b, s) => new { b, s.CinemaId })
                .GroupBy(bs => bs.CinemaId)
                .Select(g => new Statistic_Movies_Cinemas
                {
                    Name = _context.Cinemas.Where(c => c.CinemaId == g.Key).Select(c => c.Name).FirstOrDefault(),
                    Revenue = g.Sum(bs => bs.b.TotalPrice)
                })
                .OrderByDescending(g => g.Revenue)
                .ToList();

            // Tạo model và truyền các thống kê vào view
            var model = new OverviewModel
            {
                CinemasRevenue = cinemasRevenue
            };

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(model);
        }
        public IActionResult TotalRevenue(DateTime? startDate, DateTime? endDate)
        {
            // Đặt giá trị mặc định cho ngày bắt đầu và ngày kết thúc nếu chúng không được truyền vào
            startDate ??= DateTime.Now.AddMonths(-3);
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

        [HttpPost]
        public IActionResult ExportChartDataToExcel_NewUsers([FromBody] ChartExportRequest_Users_Tickets request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập giấy phép phi thương mại

            using (var package = new ExcelPackage())
            {
                // Thêm log để kiểm tra dữ liệu nhận được
                Console.WriteLine(JsonConvert.SerializeObject(request));
                // Sheet thống kê theo ngày
                var daySheet = package.Workbook.Worksheets.Add("Thống kê theo ngày");
                daySheet.Cells[1, 1].Value = "Ngày";
                daySheet.Cells[1, 2].Value = "Số người dùng mới";

                for (int i = 0; i < request.DayChartData.Labels.Count; i++)
                {
                    daySheet.Cells[i + 2, 1].Value = request.DayChartData.Labels[i];
                    daySheet.Cells[i + 2, 2].Value = request.DayChartData.Data[i];
                }

                // Sheet thống kê theo tháng
                var monthSheet = package.Workbook.Worksheets.Add("Thống kê theo tháng");
                monthSheet.Cells[1, 1].Value = "Tháng";
                monthSheet.Cells[1, 2].Value = "Số người dùng mới";

                for (int i = 0; i < request.MonthChartData.Labels.Count; i++)
                {
                    monthSheet.Cells[i + 2, 1].Value = request.MonthChartData.Labels[i];
                    monthSheet.Cells[i + 2, 2].Value = request.MonthChartData.Data[i];
                }

                // Xuất file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewUsers.xlsx");
            }
        }
        [HttpPost]
        public IActionResult ExportChartDataToExcel_TotalTickets([FromBody] ChartExportRequest_Users_Tickets request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập giấy phép phi thương mại

            using (var package = new ExcelPackage())
            {
                // Thêm log để kiểm tra dữ liệu nhận được
                Console.WriteLine(JsonConvert.SerializeObject(request));
                // Sheet thống kê theo ngày
                var daySheet = package.Workbook.Worksheets.Add("Thống kê theo ngày");
                daySheet.Cells[1, 1].Value = "Ngày";
                daySheet.Cells[1, 2].Value = "Số vé bán ra";

                for (int i = 0; i < request.DayChartData.Labels.Count; i++)
                {
                    daySheet.Cells[i + 2, 1].Value = request.DayChartData.Labels[i];
                    daySheet.Cells[i + 2, 2].Value = request.DayChartData.Data[i];
                }

                // Sheet thống kê theo tháng
                var monthSheet = package.Workbook.Worksheets.Add("Thống kê theo tháng");
                monthSheet.Cells[1, 1].Value = "Tháng";
                monthSheet.Cells[1, 2].Value = "Số vé bán ra";

                for (int i = 0; i < request.MonthChartData.Labels.Count; i++)
                {
                    monthSheet.Cells[i + 2, 1].Value = request.MonthChartData.Labels[i];
                    monthSheet.Cells[i + 2, 2].Value = request.MonthChartData.Data[i];
                }

                // Xuất file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TotalTickets.xlsx");
            }
        }
        [HttpPost]
        public IActionResult ExportChartDataToExcel_TotalRevenue([FromBody] ChartExportRequest_Revenue request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập giấy phép phi thương mại

            using var package = new ExcelPackage();
            // Thêm log để kiểm tra dữ liệu nhận được
            Console.WriteLine(JsonConvert.SerializeObject(request));
            // Sheet thống kê theo ngày
            var daySheet = package.Workbook.Worksheets.Add("Thống kê theo ngày");
            daySheet.Cells[1, 1].Value = "Ngày";
            daySheet.Cells[1, 2].Value = "Tổng doanh thu";

            for (int i = 0; i < request.DayChartData.Labels.Count; i++)
            {
                daySheet.Cells[i + 2, 1].Value = request.DayChartData.Labels[i];
                daySheet.Cells[i + 2, 2].Value = request.DayChartData.Data[i];
            }

            // Sheet thống kê theo tháng
            var monthSheet = package.Workbook.Worksheets.Add("Thống kê theo tháng");
            monthSheet.Cells[1, 1].Value = "Tháng";
            monthSheet.Cells[1, 2].Value = "Tổng doanh thu";

            for (int i = 0; i < request.MonthChartData.Labels.Count; i++)
            {
                monthSheet.Cells[i + 2, 1].Value = request.MonthChartData.Labels[i];
                monthSheet.Cells[i + 2, 2].Value = request.MonthChartData.Data[i];
            }

            // Xuất file Excel
            var stream = new MemoryStream(package.GetAsByteArray());
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TotalRevenue.xlsx");
        }
        [HttpPost]
        public IActionResult ExportChartDataToExcel_MoviesRevenue([FromBody] ChartExportRequest_Movies_Cinemas request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập giấy phép phi thương mại

            using (var package = new ExcelPackage())
            {
                // Thêm log để kiểm tra dữ liệu nhận được
                Console.WriteLine(JsonConvert.SerializeObject(request));
                // Sheet thống kê
                var moviesSheet = package.Workbook.Worksheets.Add("Thống kê theo phim");
                moviesSheet.Cells[1, 1].Value = "Tên phim";
                moviesSheet.Cells[1, 2].Value = "Doanh thu phim";

                for (int i = 0; i < request.RevenueChartData.Labels.Count; i++)
                {
                    moviesSheet.Cells[i + 2, 1].Value = request.RevenueChartData.Labels[i];
                    moviesSheet.Cells[i + 2, 2].Value = request.RevenueChartData.Data[i];
                }

                // Xuất file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MoviesRevenue.xlsx");
            }
        }
        [HttpPost]
        public IActionResult ExportChartDataToExcel_CinemasRevenue([FromBody] ChartExportRequest_Movies_Cinemas request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Thiết lập giấy phép phi thương mại

            using (var package = new ExcelPackage())
            {
                // Thêm log để kiểm tra dữ liệu nhận được
                Console.WriteLine(JsonConvert.SerializeObject(request));
                // Sheet thống kê
                var cinemasSheet = package.Workbook.Worksheets.Add("Thống kê theo rạp");
                cinemasSheet.Cells[1, 1].Value = "Tên rạp";
                cinemasSheet.Cells[1, 2].Value = "Doanh thu rạp";

                for (int i = 0; i < request.RevenueChartData.Labels.Count; i++)
                {
                    cinemasSheet.Cells[i + 2, 1].Value = request.RevenueChartData.Labels[i];
                    cinemasSheet.Cells[i + 2, 2].Value = request.RevenueChartData.Data[i];
                }

                // Xuất file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CinemasRevenue.xlsx");
            }
        }
        public class ChartExportRequest_Users_Tickets
        {
            public ChartData_Users_Tickets DayChartData { get; set; }
            public ChartData_Users_Tickets MonthChartData { get; set; }
        }
        public class ChartData_Users_Tickets
        {
            public List<string> Labels { get; set; }
            public List<int> Data { get; set; }
        }
        public class ChartExportRequest_Revenue
        {
            public ChartData_Revenue DayChartData { get; set; }
            public ChartData_Revenue MonthChartData { get; set; }
        }
        public class ChartData_Revenue
        {
            public List<string> Labels { get; set; }
            public List<decimal> Data { get; set; }
        }
        public class ChartExportRequest_Movies_Cinemas
        {
            public ChartData_Movies_Cinemas RevenueChartData { get; set; }
        }
        public class ChartData_Movies_Cinemas
        {
            public List<string> Labels { get; set; }
            public List<decimal> Data { get; set; }
        }
    }
}
