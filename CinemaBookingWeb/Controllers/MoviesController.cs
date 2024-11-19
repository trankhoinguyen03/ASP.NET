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

        public IActionResult MoviesDangChieu()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }


        //public async Task<IActionResult> Index()
        //{
        //    var movies = await _context.Movies
        //        .Where(m => m.Status == 1)
        //        .ToListAsync();

        //    return View(movies);
        //}


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


        //public async Task<IActionResult> Index()
        //{
        //    // Chỉ hiển thị các phim có Status = 1 (Hiển thị)
        //    return View(await _context.Movies.Where(m => m.Status == 1).ToListAsync());
        //}

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Movies.ToListAsync());
        //}


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl")] Movies movie)
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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie)
        //{
        //    if (id != movie.MovieId) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(movie);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MovieExists(movie.MovieId)) return NotFound();
        //            else throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(movie);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile fileInput)
        {
            if (id != movie.MovieId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu có file upload
                    if (fileInput != null && fileInput.Length > 0)
                    {
                        // Đường dẫn lưu file
                        var fileName = Path.GetFileName(fileInput.FileName);
                        var filePath = Path.Combine("wwwroot/movies_img", fileName);

                        // Lưu file vào hệ thống
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await fileInput.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn ảnh
                        movie.ImageUrl = "/movies_img/" + fileName;
                    }

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile ImageFile)
        //{
        //    if (id != movie.MovieId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Nếu người dùng upload ảnh mới
        //            if (ImageFile != null && ImageFile.Length > 0)
        //            {
        //                // Lưu ảnh vào thư mục wwwroot/images
        //                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/movies_img", ImageFile.FileName);
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await ImageFile.CopyToAsync(stream);
        //                }

        //                // Cập nhật đường dẫn ảnh
        //                movie.ImageUrl = "/movies_img/" + ImageFile.FileName;
        //            }

        //            _context.Update(movie);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MovieExists(movie.MovieId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(movie);
        //}

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }


    }
}
