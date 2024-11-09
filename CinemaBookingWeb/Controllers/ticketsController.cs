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
            ViewBag.Seats = null;
            ViewBag.Showtimes = null;
            ViewBag.bookedSeats = null;
            ViewBag.bookSeats = null;
            ViewBag.temp = null;
            if (cinema == null)
            {
                ViewBag.Movies = null;
            }
            else
            {
                ViewBag.Movies = moviesQuery
                                    .Where(m => m.Showtimes.Any(s => s.Cinema.City == cinema.City))
                                    .ToList();

                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == currentTicket.MovieId);
                if (movie != null)
                {
                    ViewBag.Showtimes= _context.Showtimes
                              .Include(s => s.Movie)
                              .Include(s => s.Cinema)
                              .Where(s => s.Cinema.City == cinema.City && s.Movie.Title == movie.Title)
                              .ToList();
                    
                    

                    
                    if (currentTicket.ShowtimeId!=null)
                    {
                        var bookedSeats = _context.BookingDetails
                                    .Where(bd => bd.Booking.ShowtimeId == currentTicket.ShowtimeId)
                                    .ToList();
                        /*if (currentTicket.seats.Count > 0)
                        {
                            ViewBag.bookSeats = currentTicket.seats.ToList();
                        }*/
                        ViewBag.Seats = _context.Seats.ToList();
                        ViewBag.bookedSeats= bookedSeats;

                    }

                }
                    
            }
            ViewBag.Combo=_context.Combos.ToList();

            ViewBag.Cinemas = _context.Cinemas
                                       .GroupBy(c => c.City)
                                       .Select(g => g.First())
                                       .ToList();
            if (currentTicket.seats !=null)
            {
                ViewBag.temp = currentTicket.seats.ToList();
            }
            if (currentTicket.BookingCombo != null)
            {
                ViewBag.temp22 = currentTicket.BookingCombo.ToList();
            }


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
            var updatedTicket = Ticket;
            updatedTicket.ShowtimeId = id;

            Ticket = updatedTicket;


            return RedirectToAction("Index");
        }
        public IActionResult ChoiceSeats(int id)
        {
            var temp = -1;
            var updatedTicket= Ticket;
            if (updatedTicket.seats == null)
            {
                updatedTicket.seats = new List<int>();
            }
            else
            {
                for(var i=0;i<updatedTicket.seats.Count;i++)
                {
                    if (updatedTicket.seats[i] == id)
                    {
                        temp = i;
                        break;
                    }
                }

            }
           
            if (temp<0)
            {
                updatedTicket.seats.Add(id);
            }
            else
            {
                updatedTicket.seats.RemoveAt(temp);
            }
            
            Ticket = updatedTicket;
            return RedirectToAction("Index");
        }
        public IActionResult ChoiceCombos(Dictionary<int, int> quantities)
        {
            var updatedTicket = Ticket;
            
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
        public IActionResult Checkout()
        {
            var booking = new Bookings
            {
                UserId = 1,
                ShowtimeId = Ticket.ShowtimeId ?? 0,
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
                        SeatId = item,
                        Price = 0,
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
            

            return RedirectToAction("Index");
        }

    }
}


