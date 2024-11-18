using CinemaBookingWeb.Data;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Helpers;
using CinemaBookingWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Sockets;

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
            var currentTicket = Ticket;
            var cinema = _context.Cinemas.FirstOrDefault(c => c.CinemaId == currentTicket.CinemaId);

            var moviesQuery = _context.Movies
                                      .Include(m => m.Showtimes)
                                      .AsQueryable();

            if (currentTicket.viewTemp <=0 || currentTicket.viewTemp==null)
            {
                currentTicket.viewTemp = 1;
            }
            ViewBag.viewTemp=currentTicket.viewTemp;
            if (currentTicket.date == null)
            {
                currentTicket.date= DateOnly.FromDateTime(DateTime.Now);
                
            }
            ViewBag.choicedate = currentTicket.date;
            if (cinema == null)
            {
                /*ViewBag.Movies = null;
                ViewBag.tempId = null;
                ViewBag.Seats = null;
                ViewBag.Showtimes = null;
                ViewBag.bookedSeats = null;
                ViewBag.finalTicket = null;*/
            }
            else
            {   
                ViewBag.tempid=currentTicket.CinemaId;
                ViewBag.Movies =moviesQuery
                                    .Where(m => m.Showtimes.Any(s => s.Cinema.City == cinema.City))
                                    .ToList();

                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == currentTicket.MovieId);
                if (movie != null)
                {
                    ViewBag.Showtimes= _context.Showtimes
                              .Include(s => s.Movie)
                              .Include(s => s.Cinema)
                              .Where(s => s.Cinema.City == cinema.City && s.Movie.Title == movie.Title && DateOnly.FromDateTime( s.StartTime.Date)== currentTicket.date)
                              .GroupBy(s => new
                              {
                                  s.Cinema.CinemaId,
                                  s.Cinema.Name,
                                  s.Cinema.Location
                              })
                              .Select(group=>new
                              {
                                  cinemaId=group.Key.CinemaId,
                                  cinemaName=group.Key.Name,
                                  location=group.Key.Location,
                                  Showtimes=group.Select(s => new
                                  {
                                      showtimesId=s.ShowtimeId,
                                      startTime=s.StartTime.ToString("HH:mm"),
                                      endTime=s.EndTime.ToString("HH:mm"),
                                      hall=s.Hall,
                                      date= s.StartTime.ToString("dd/MM/yyyy"),
                                      price = s.Price

                                  }).ToList()
                              })
                              .ToList();
                    
                    

                  /*  xoa*/
                    if (currentTicket.ShowtimeId!=null)
                    {
                        var bookedSeats = _context.BookingDetails
                                    .Where(bd => bd.Booking.ShowtimeId == currentTicket.ShowtimeId)
                                    .ToList();
                      
                        ViewBag.Seats = _context.Seats.ToList();
                        ViewBag.bookedSeats= bookedSeats;

                    }
                    ViewBag.finalTicket = currentTicket;

                }
                    
            }
            ViewBag.Combo=_context.Combos.ToList();

            ViewBag.Cinemas = _context.Cinemas
                                       .GroupBy(c => c.City)
                                       .Select(g => g.First())
                                       .ToList();
            return View();

        }

        public IActionResult ChoiceCity(int id)
        {
            var updatedTicket = Ticket;
            updatedTicket.CinemaId = id;
            Ticket = updatedTicket;

            return RedirectToAction("Index");
        }

        public IActionResult ChoiceMovie(int id)
        {
            var updatedTicket = Ticket;
            updatedTicket.MovieId = id;
            Ticket = updatedTicket;

            return RedirectToAction("Index");
        }

        public IActionResult ChoiceShowTime(int id)
        {
            tickets updatedTicket = Ticket;
            updatedTicket.ShowtimeId= id;
            updatedTicket.viewTemp = 2;
            Ticket = updatedTicket;


            return RedirectToAction("Index");
        }

        public IActionResult ChoiceDay(int days)
        {
            var updatedTicket = Ticket;
            updatedTicket.date = DateOnly.FromDateTime( DateTime.Today.AddDays(days));
            Ticket=updatedTicket;
            return RedirectToAction("Index");
        }
        public IActionResult ChoiceSeats(Dictionary<int,decimal>bookedSeats)
        {
            

            var updatedTicket= Ticket;
            updatedTicket.viewTemp = 3;
            if (updatedTicket.seats == null)
            {
                updatedTicket.seats = new List<BookingDetails> ();
            }
            foreach(var temp in bookedSeats)
            {
                if (temp.Value != 0)
                {
                    var seat = new BookingDetails();
                    seat.SeatId = temp.Key;
                    seat.Price = temp.Value;
                    updatedTicket.seats.Add(seat);
                }
            }
           
            Ticket = updatedTicket;
            return RedirectToAction("Index");
        }
        public IActionResult ChoiceCombos(Dictionary<int, int> quantities)
        {
            var updatedTicket = Ticket;
            updatedTicket.viewTemp = 4;
            updatedTicket.BookingCombo = new List<BookingCombos> { };
            
            foreach (var quant in quantities)
            {
                if (quant.Value > 0)
                {
                    var combo = new BookingCombos { ComboId = quant.Key, Quantity = quant.Value };
                    updatedTicket.BookingCombo.Add(combo);
                }
            }
            Ticket = updatedTicket;
            return RedirectToAction("Index");
        }
        
        public IActionResult choiceView(int id)
        {
            var updatedTicket = Ticket;
            updatedTicket.viewTemp = id;
            Ticket = updatedTicket;
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var temp = Ticket;
            var booking = new Bookings
            {
                UserId = 1,
                ShowtimeId = temp.ShowtimeId,
                BookingDate=DateTime.Now,
                TotalPrice = 0,
                Status = 2
            };

            _context.Database.BeginTransaction();
            
            {
                _context.Database.CommitTransaction();
                _context.Add(booking);
                _context.SaveChanges();

                var bookingdetails=new List<BookingDetails>();
                foreach(var item in Ticket.seats)
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
                

                var bookingcombos=new List<BookingCombos>();
                foreach(var item in Ticket.BookingCombo)
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

            Ticket = null;
            return RedirectToAction("Index");
        }

    }
}


