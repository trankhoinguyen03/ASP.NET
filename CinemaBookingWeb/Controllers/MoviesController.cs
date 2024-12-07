using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBookingWeb.Models;
using CinemaBookingWeb.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;



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





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile? fileInput)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (fileInput != null && fileInput.Length > 0)
        //            {

        //                if (fileInput.Length > 5 * 1024 * 1024)
        //                {
        //                    ModelState.AddModelError("ImageUrl", "Kích thước ảnh không được vượt quá 5MB.");
        //                    return View(movie);
        //                }

        //                // Lưu file
        //                var fileName = Path.GetFileName(fileInput.FileName);
        //                var filePath = Path.Combine("wwwroot/img/movies_img", fileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await fileInput.CopyToAsync(stream);
        //                }

        //                // Cập nhật đường dẫn ảnh
        //                movie.ImageUrl = "/img/movies_img/" + fileName;
        //            }

        //            movie.Status = 1; // Mặc định trạng thái
        //            _context.Add(movie);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra: " + ex.Message);
        //        }
        //    }
        //    return View(movie);
        //}




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

                        // Đường dẫn lưu file
                        var fileName = Path.GetFileNameWithoutExtension(fileInput.FileName);
                        var extension = Path.GetExtension(fileInput.FileName).ToLower();
                        var filePath = Path.Combine("wwwroot/img/movies", fileName + extension);

                        // Resize ảnh về kích thước chuẩn (VD: 300x450)
                        using (var image = Image.Load(fileInput.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Crop,
                                Size = new Size(500, 750) // Kích thước ảnh chuẩn
                            }));

                            // Lưu ảnh vào hệ thống
                            await image.SaveAsync(filePath);
                        }

                        // Cập nhật đường dẫn ảnh
                        movie.ImageUrl = "/img/movies/" + fileName + extension;
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



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile? fileInput)
        //{
        //    // Kiểm tra ModelState
        //    if (!ModelState.IsValid)
        //    {
        //        return View(movie);
        //    }

        //    if (fileInput != null && fileInput.Length > 0)
        //    {
   
        //        // Kiểm tra kích thước file (giới hạn 5MB)
        //        if (fileInput.Length > 5 * 1024 * 1024)
        //        {
        //            ModelState.AddModelError("ImageUrl", "Kích thước ảnh không được vượt quá 5MB.");
        //            return View(movie);
        //        }

        //        // Lưu file
        //        var fileName = Path.GetFileName(fileInput.FileName);
        //        var filePath = Path.Combine("wwwroot/img/movies_img", fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await fileInput.CopyToAsync(stream);
        //        }

        //        movie.ImageUrl = "/img/movies_img/" + fileName;
        //    }

        //    try
        //    {
        //        _context.Update(movie);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(movie.MovieId))
        //        {
        //            return NotFound();
        //        }
        //        throw;
        //    }
        //}






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId, Title, Description, Duration, Rating, ReleaseDate, Genre, Language, TrailerUrl, ImageUrl, Status")] Movies movie, IFormFile? fileInput)
        {
            if (id != movie.MovieId) return NotFound();

            if (!ModelState.IsValid) return View(movie);

            if (fileInput != null && fileInput.Length > 0)
            {


                // Kiểm tra kích thước file (giới hạn 5MB)
                if (fileInput.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageUrl", "Kích thước ảnh không được vượt quá 5MB.");
                    return View(movie);
                }

                // Đường dẫn lưu file
                var fileName = Path.GetFileNameWithoutExtension(fileInput.FileName);
                var extension = Path.GetExtension(fileInput.FileName).ToLower();
                var filePath = Path.Combine("wwwroot/img/movies", fileName + extension);

                // Resize ảnh về kích thước chuẩn (VD: 300x450)
                try
                {
                    // Resize ảnh về kích thước chuẩn (300x450)
                    using (var image = SixLabors.ImageSharp.Image.Load(fileInput.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(500, 750) // Kích thước chuẩn
                        }));

                        // Lưu ảnh vào thư mục
                        await image.SaveAsync(filePath);
                    }

                    // Cập nhật đường dẫn ảnh
                    movie.ImageUrl = "/img/movies/" + fileName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xử lý ảnh: " + ex.Message);
                    return View(movie);
                }
            }

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.MovieId)) return NotFound();
                throw;
            }
        }



        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }


    }
}
