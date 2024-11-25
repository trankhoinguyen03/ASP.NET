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
        public async Task<IActionResult> Create([Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile fileInput)
        {
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
                    movie.Status = 1;
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần
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
        //public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile fileInput)
        //{
        //    if (id != movie.MovieId) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if(fileInput!=null && fileInput.Length> 0)
        //            {
        //                // Xử lý lưu file mới
        //                var fileName = Path.GetFileName(fileInput.FileName);
        //                var filePath = Path.Combine("wwwroot/movies_img", fileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await fileInput.CopyToAsync(stream);
        //                }

        //                // Cập nhật đường dẫn ảnh
        //                movie.ImageUrl = "/movies_img/" + fileName;
        //            }

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




        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }


    }
}
