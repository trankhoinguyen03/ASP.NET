using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBookingWeb.Models;
using CinemaBookingWeb.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;



namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {

            var movies = from m in _context.Movies
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }

            return View(await movies.ToListAsync());
        }




        public IActionResult Create()
        {
            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile? fileInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (fileInput != null && fileInput.Length > 0)
                    {

                        if (fileInput.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageUrl", "Kích thước ảnh không được vượt quá 5MB.");
                            return View(movie);
                        }

                        // Lưu file
                        var fileName = Path.GetFileName(fileInput.FileName);
                        var filePath = Path.Combine("wwwroot/img/movies_img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileInput.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn ảnh
                        movie.ImageUrl = "/img/movies_img/" + fileName;
                    }

                    movie.Status = 1; // Mặc định trạng thái
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra: " + ex.Message);
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile? fileInput)
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            if (fileInput != null && fileInput.Length > 0)
            {
                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(fileInput.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageUrl", "Ảnh phải có định dạng jpg, jpeg hoặc png.");
                    return View(movie);
                }

                // Kiểm tra kích thước file (giới hạn 5MB)
                if (fileInput.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageUrl", "Kích thước ảnh không được vượt quá 5MB.");
                    return View(movie);
                }

                // Lưu file
                var fileName = Path.GetFileName(fileInput.FileName);
                var filePath = Path.Combine("wwwroot/img/movies_img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileInput.CopyToAsync(stream);
                }

                movie.ImageUrl = "/img/movies_img/" + fileName;
            }

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.MovieId))
                {
                    return NotFound();
                }
                throw;
            }
        }



        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }


    }
}
