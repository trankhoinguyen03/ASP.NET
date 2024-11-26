using System.Linq;
using System.Threading.Tasks;
using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CinemaBookingWeb.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemasController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString)
        {

            var cinema = from m in _context.Cinemas
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                cinema = cinema.Where(m => m.Name.Contains(searchString));
            }

            return View(await cinema.ToListAsync());
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(c => c.CinemaId == id);

            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CinemaId, Name, Location, Phone, City")] Cinemas cinema)
        {
            if (ModelState.IsValid)
            {
                // set = 1 ở đây
                cinema.Status = 1;
                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CinemaId, Name, Location, Phone, City, Status")] Cinemas cinema)
        {
            if (id != cinema.CinemaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.CinemaId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }


        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.CinemaId == id);
        }
    }
}