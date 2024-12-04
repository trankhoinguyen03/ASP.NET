using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Helpers;
using CinemaBookingWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Sockets;
using System.Linq;
using NuGet.Protocol;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CinemaBookingWeb.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string TICKET_KEY = "MYTICKET";

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private tickets Ticket
        {
            get => HttpContext.Session.Get<tickets>(TICKET_KEY) ?? new tickets();
            set => HttpContext.Session.Set(TICKET_KEY, value);
        }

        public IActionResult Index()
        {

            Ticket = new tickets();

            //cần
            ViewBag.Cinemas = _context.Cinemas
                                       .GroupBy(c => c.City)
                                       .Select(g => g.First())
                                       .ToList();







            return View();
        }
        [HttpGet]
        public JsonResult GetCinemasByCity(string city)
        {
            if (!int.TryParse(city, out int cityId))
            {
                return Json(new List<object>()); // Trả về danh sách trống nếu không hợp lệ
            }

            var tempcinema = _context.Cinemas.FirstOrDefault(c => c.CinemaId == cityId);

            var today = DateTime.Today;
            var tem = _context.Movies.ToList();

            var moviesInCity = _context.Showtimes
                            .Join(
                                _context.Cinemas,          // Thực hiện join với bảng Cinemas
                                s => s.CinemaId,           // Khóa ngoại từ Showtimes
                                c => c.CinemaId,           // Khóa chính của Cinemas
                                (s, c) => new { Showtime = s, Cinema = c }
                            )
                            .Where(sc => sc.Cinema.City == tempcinema.City && sc.Showtime.StartTime.Date >= today) // Lọc theo thành phố và ngày chiếu
                            .Join(
                                _context.Movies,           // Thực hiện join với bảng Movies
                                sc => sc.Showtime.MovieId, // Khóa ngoại từ Showtimes
                                m => m.MovieId,            // Khóa chính của Movies
                                (sc, m) => new
                                {
                                    m.MovieId,
                                    m.Title,
                                    m.ImageUrl,
                                    m.Genre,
                                    Cinema = sc.Cinema.Name,
                                    StartTime = sc.Showtime.StartTime
                                }
                            )
                            .GroupBy(movie => movie.Title) // Nhóm các phim theo tiêu đề để loại bỏ trùng lặp
                            .Select(group => group.First()) // Lấy phim đầu tiên trong mỗi nhóm
                            .ToList();
            Ticket.cinema = null;
            var updatedTicket = Ticket;
            updatedTicket.cinema = tempcinema;
            Ticket = updatedTicket;
            return (Json(moviesInCity));
        }



        [HttpGet]
        public IActionResult ChoiceMovie(int id, int? date)
        {
            var updatedTicket = Ticket;

            var cinema = _context.Cinemas.FirstOrDefault(c => c.CinemaId == updatedTicket.cinema.CinemaId);
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);

            updatedTicket.date = DateOnly.FromDateTime(DateTime.Today.AddDays(date ?? 0));


            var showtimes = _context.Showtimes
                            .Include(s => s.Movie)
                            .Include(s => s.Cinema)
                               .Where(s => s.Cinema.City == cinema.City && s.Movie.Title == movie.Title && DateOnly.FromDateTime(s.StartTime.Date) == updatedTicket.date)
                               .GroupBy(s => new
                               {
                                   s.Cinema.CinemaId,
                                   s.Cinema.Name,
                                   s.Cinema.Location

                               })
                               .Select(group => new
                               {
                                   cinemaId = group.Key.CinemaId,
                                   cinemaName = group.Key.Name,
                                   location = group.Key.Location,
                                   imageUrl = movie.ImageUrl,
                                   title = movie.Title,
                                   Showtimes = group.Select(s => new
                                   {
                                       showtimeId = s.ShowtimeId,
                                       startTime = s.StartTime.ToString("HH:mm"),
                                       endTime = s.EndTime.ToString("HH:mm"),
                                       hall = s.Hall,
                                       date = s.StartTime.ToString("dd/MM/yyyy"),
                                       price = s.Price
                                   }).ToList()
                               })
                               .ToList();


            Ticket.movie = null;
            updatedTicket.movie = movie;
            Ticket = updatedTicket;
            return Json(showtimes);
        }
        [HttpGet]
        public IActionResult ChoiceShowTime(int id)
        {

            Ticket.showtime = null;
            Ticket.cinema = null;
            Ticket.movie = null;
            tickets updatedTicket = Ticket;
            var tempshowtime = _context.Showtimes.FirstOrDefault(s => s.ShowtimeId == id);
            var tempmovie = _context.Movies.FirstOrDefault(s => s.MovieId == tempshowtime.MovieId);
            var tempcinema = _context.Cinemas.FirstOrDefault(s => s.CinemaId == tempshowtime.CinemaId);
            updatedTicket.showtime = tempshowtime;
            updatedTicket.movie = tempmovie;
            updatedTicket.cinema = tempcinema;
            Ticket = updatedTicket;


            return Json(updatedTicket);
        }

        public IActionResult ViewChoiceSeat()
        {
            Ticket.totalPrice = 0;
            Ticket.priceCombo = 0;
            var bookedSeats = _context.BookingDetails
                                   .Where(bd => bd.Booking.ShowtimeId == Ticket.showtime.ShowtimeId)
                                   .ToList();
            ViewBag.Seats = _context.Seats.ToList();
            ViewBag.bookedSeats = bookedSeats;
            DateTime startTime = Ticket.showtime.StartTime;

            // Định dạng thời gian (giờ:phút)
            string formattedTime = startTime.ToString("HH:mm");

            // Định dạng ngày tháng (ngày/tháng/năm)
            string formattedDate = startTime.ToString("dd/MM/yyyy");
            Dictionary<string, string> viewTicket = new Dictionary<string, string>
                                                                                        {
                                                                                            { "CinemaName", Ticket.cinema.Name },
                                                                                            { "CinemaLocation", Ticket.cinema.Location },
                                                                                            { "MovieTitle", Ticket.movie.Title },
                                                                                            { "MovieImageUrl", Ticket.movie.ImageUrl },
                                                                                            { "ShowtimeHall", Ticket.showtime.Hall },
                                                                                            { "FormattedTime", formattedTime },
                                                                                            { "FormattedDate", formattedDate },
                                                                                            {"price",Ticket.showtime.Price.ToString() },
                                                                                        };


            ViewBag.ViewTicket = viewTicket;
            return View();
        }

        [HttpPost]
        public IActionResult ChoiceSeats(Dictionary<int, decimal> bookedSeats)
        {
            
            decimal temptotalprice = 0;
            var updatedTicket = Ticket;
            updatedTicket.seats = new List<BookingDetails>();
            foreach (var temp in bookedSeats)
            {
                if (temp.Value != 0)
                {
                    var seat = new BookingDetails();
                    seat.SeatId = temp.Key;
                    seat.Price = temp.Value;
                    temptotalprice = temptotalprice + temp.Value;
                    updatedTicket.seats.Add(seat);
                }
            }
            updatedTicket.totalPrice = temptotalprice;
            Ticket = updatedTicket;

            if (ViewBag.Seats == null)
            {
                ViewBag.Seats = _context.Seats.ToList();
            }
            if (ViewBag.Combo == null)
            {
                ViewBag.Combo = _context.Combos.ToList();
            }

            return View(updatedTicket);

        }
        [HttpPost]
        public IActionResult ChoiceCombos(Dictionary<int, decimal> quantities)
        {
            tickets updatedTicket1 = Ticket;

            if (quantities != null)
            {

                decimal temppriceCombo = 0;
                decimal totalPrice = updatedTicket1.totalPrice - updatedTicket1.priceCombo;
                updatedTicket1.priceCombo = 0;
                updatedTicket1.BookingCombo = new List<BookingCombos> { };

                foreach (var quant in quantities)
                {
                    if (quant.Value > 0)
                    {
                        var combo = new BookingCombos { ComboId = quant.Key, Quantity = (int?)quant.Value };
                        updatedTicket1.BookingCombo.Add(combo);
                        var pricecombo = _context.Combos.Where(s => s.ComboId == quant.Key).FirstOrDefault();
                        temppriceCombo += pricecombo.Price * quant.Value;
                    }
                }
                updatedTicket1.totalPrice = totalPrice + temppriceCombo;
                updatedTicket1.priceCombo = temppriceCombo;
                Ticket = updatedTicket1;
            }
            if (ViewBag.Seats == null)
            {
                ViewBag.Seats = _context.Seats.ToList();
            }
            if (ViewBag.Combo == null)
            {
                ViewBag.Combo = _context.Combos.ToList();
            }

            return View(updatedTicket1);
        }

        public IActionResult choiceView(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }
            if (id == 2)
            {
                return RedirectToAction("ViewChoiceSeat");
            }
            if (id == 3)
            {
                if (ViewBag.Seats == null)
                {
                    ViewBag.Seats = _context.Seats.ToList();
                }
                if (ViewBag.Combo == null)
                {
                    ViewBag.Combo = _context.Combos.ToList();
                }
                return View("ChoiceSeats", Ticket);
            }
            return NoContent();
        }
        public IActionResult Checkout()
        {
            tickets temp = Ticket;

            Bookings booking = new Bookings
            {
                UserId = 1,
                ShowtimeId = temp.showtime.ShowtimeId,
                BookingDate = DateTime.Now,
                TotalPrice = Ticket.totalPrice,
                Status = 2
            };

            _context.Database.BeginTransaction();

            {
                _context.Database.CommitTransaction();
                _context.Add(booking);
                _context.SaveChanges();

                var bookingdetails = new List<BookingDetails>();
                foreach (var item in Ticket.seats)
                {
                    bookingdetails.Add(new BookingDetails
                    {
                        BookingId = booking.BookingId,
                        SeatId = item.SeatId,
                        Price = item.Price,
                        Status = 1
                    });
                }
                _context.AddRange(bookingdetails);


                var bookingcombos = new List<BookingCombos>();
                foreach (var item in Ticket.BookingCombo)
                {
                    bookingcombos.Add(new BookingCombos
                    {
                        BookingId = booking.BookingId,
                        ComboId = item.ComboId,
                        Quantity = item.Quantity,
                        Status = 1
                    });
                }
                _context.AddRange(bookingcombos);
                _context.SaveChanges();



            }

            Ticket = new tickets();
            return RedirectToAction("Index");
        }

    }
}


