using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBookingWeb.Models;
using CinemaBookingWeb.Data;
using System.Text;



namespace CinemaBookingWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ExportMoviesTable()
        {
            var movies = await _context.Movies
                .Select(m => new
                {
                    m.MovieId,
                    m.Title,
                    m.Genre,
                    m.ReleaseDate,
                    m.Duration
                })
                .ToListAsync();

            StringBuilder html = new StringBuilder();
            html.Append("<table border='1'>");
            html.Append("<tr>");
            html.Append("<th>ID</th>");
            html.Append("<th>Title</th>");
            html.Append("<th>Genre</th>");
            html.Append("<th>Release Date</th>");
            html.Append("<th>Duration (minutes)</th>");
            html.Append("</tr>");

            foreach (var movie in movies)
            {
                html.Append("<tr>");
                html.Append($"<td>{movie.MovieId}</td>");
                html.Append($"<td>{movie.Title}</td>");
                html.Append($"<td>{movie.Genre}</td>");
                html.Append($"<td>{movie.ReleaseDate.ToShortDateString()}</td>");
                html.Append($"<td>{movie.Duration}</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");

            return Content(html.ToString(), "text/html");
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Genre,Director,Cast,ReleaseDate,Duration,Rating,TrailerUrl,ImageUrl")] Movies movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Description,Genre,Director,Cast,ReleaseDate,Duration,Rating,TrailerUrl,ImageUrl")] Movies movie)
        {
            if (id != movie.MovieId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}