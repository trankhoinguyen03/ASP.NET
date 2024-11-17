using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingWeb.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowtimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Showtimes
        public IActionResult Index()
        {
            var showtimes = _context.Showtimes
                .Include(s => s.Movie)  // Movie
                .Include(s => s.Cinema) // Cinema
                .ToList(); 
            return View(showtimes);
        }

        public IActionResult Create()
        {
            return View();
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Showtimes showtime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(showtime);
                _context.SaveChangesAsync();  
                return RedirectToAction(nameof(Index));
            }
            return View(showtime);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = _context.Showtimes.Find(id);
            if (showtime == null)
            {
                return NotFound();
            }
            return View(showtime);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Showtimes showtime)
        {
            if (id != showtime.ShowtimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showtime);
                    _context.SaveChanges();  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowtimesExists(showtime.ShowtimeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(showtime);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .FirstOrDefault(m => m.ShowtimeId == id);
            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                _context.SaveChanges();  
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ShowtimesExists(int id)
        {
            return _context.Showtimes.Any(e => e.ShowtimeId == id);
        }
    }
}
