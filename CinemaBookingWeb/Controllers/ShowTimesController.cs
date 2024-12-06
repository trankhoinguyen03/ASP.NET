using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShowtimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowtimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Showtimes
        public IActionResult Index(string searchString)
        {
            // Lấy danh sách tất cả suất chiếu
            var showtimes = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .AsNoTracking();

            // Nếu có tham số tìm kiếm, lọc theo tiêu chí
            if (!string.IsNullOrEmpty(searchString))
            {
                showtimes = showtimes.Where(s =>
                    s.Movie.Title.Contains(searchString) ||
                    s.Cinema.Name.Contains(searchString));
            }

            return View(showtimes.ToList());
        }

        // GET: Create
        public IActionResult Create()
        {
            //ViewData["Movies"] = new SelectList(_context.Movies, "MovieId", "Title");
            //ViewData["Cinemas"] = new SelectList(_context.Cinemas, "CinemaId", "Name");
            //return View();
            // Lọc phim và rạp đang hoạt động
            ViewBag.Movies = new SelectList(_context.Movies.Where(m => m.Status == 1), "MovieId", "Title");
            ViewBag.Cinemas = new SelectList(_context.Cinemas.Where(c => c.Status == 1), "CinemaId", "Name");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Showtimes showtime)
        {
            try
            {
                Console.WriteLine($"MovieId: {showtime.MovieId}, CinemaId: {showtime.CinemaId}");
                
                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == showtime.MovieId);
                if (movie != null)
                {
                    // Tính toán EndTime bằng cách cộng Duration vào StartTime
                    showtime.EndTime = showtime.StartTime.AddMinutes(movie.Duration);
                }

                if (showtime.EndTime <= showtime.StartTime)
                {
                    ModelState.AddModelError("", "Thời gian kết thúc phải sau thời gian bắt đầu.");
                }

                var overlappingShowtime = _context.Showtimes
                    .Any(s => s.CinemaId == showtime.CinemaId &&
                              s.Hall == showtime.Hall &&
                              ((showtime.StartTime >= s.StartTime && showtime.StartTime < s.EndTime) ||
                               (showtime.EndTime > s.StartTime && showtime.EndTime <= s.EndTime)));

                if (overlappingShowtime)
                {
                    ModelState.AddModelError("", "Suất chiếu bị trùng thời gian với một suất chiếu khác.");
                }

                if (ModelState.IsValid)
                {
                    _context.Add(showtime);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

                else if (!ModelState.IsValid)
                {
                    // Hiển thị tất cả các lỗi validation trong ModelState
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi thêm suất chiếu: {ex.Message}");
            }

            ViewBag.Movies = new SelectList(_context.Movies.Where(m => m.Status == 1), "MovieId", "Title", showtime.MovieId);
            ViewBag.Cinemas = new SelectList(_context.Cinemas.Where(c => c.Status == 1), "CinemaId", "Name", showtime.CinemaId);
            return View(showtime);
        }

        public IActionResult GetMovieDuration(int movieId)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == movieId);
            if (movie != null)
            {
                return Json(new { Duration = movie.Duration });
            }

            return Json(new { Duration = 0 });
        }

        public IActionResult Edit(int? id)
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
            showtime.Price = Math.Floor(showtime.Price);
            // Lọc phim và rạp đang hoạt động
            ViewBag.Movies = new SelectList(_context.Movies.Where(m => m.Status == 1), "MovieId", "Title", showtime.MovieId);
            ViewBag.Cinemas = new SelectList(_context.Cinemas.Where(c => c.Status == 1), "CinemaId", "Name", showtime.CinemaId);

            return View(showtime);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Showtimes showtime)
        {
            // Kiểm tra nếu ID không khớp với ShowtimeId
            if (id != showtime.ShowtimeId)
            {
                return NotFound();
            }

            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == showtime.MovieId);
            if (movie != null)
            {
                // Tính toán EndTime bằng cách cộng Duration vào StartTime
                showtime.EndTime = showtime.StartTime.AddMinutes(movie.Duration);
            }

            // Kiểm tra thời gian bắt đầu và kết thúc
            if (showtime.EndTime <= showtime.StartTime)
            {
                ModelState.AddModelError("", "Thời gian kết thúc phải sau thời gian bắt đầu.");
            }

            // Kiểm tra trùng suất chiếu
            var overlappingShowtime = _context.Showtimes.Any(s =>
                s.CinemaId == showtime.CinemaId &&
                s.Hall == showtime.Hall &&
                s.ShowtimeId != showtime.ShowtimeId && // Loại trừ suất chiếu đang chỉnh sửa
                ((showtime.StartTime >= s.StartTime && showtime.StartTime < s.EndTime) ||
                 (showtime.EndTime > s.StartTime && showtime.EndTime <= s.EndTime)));

            if (overlappingShowtime)
            {
                ModelState.AddModelError("", "Suất chiếu bị trùng thời gian với một suất chiếu khác.");
            }

            // Nếu model không hợp lệ, trả về view với lỗi
            if (!ModelState.IsValid)
            {
                // Truyền lại dữ liệu Movies và Cinemas để render dropdown
                ViewBag.Movies = new SelectList(_context.Movies.Where(m => m.Status == 1), "MovieId", "Title", showtime.MovieId);
                ViewBag.Cinemas = new SelectList(_context.Cinemas.Where(c => c.Status == 1), "CinemaId", "Name", showtime.CinemaId);

                return View(showtime);
            }

            try
            {
                // Cập nhật suất chiếu
                _context.Update(showtime);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
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
            catch (Exception ex)
            {
                // Log lỗi và thông báo người dùng
                ModelState.AddModelError("", $"Đã xảy ra lỗi khi cập nhật suất chiếu: {ex.Message}");
            }

            // Truyền lại dữ liệu Movies và Cinemas khi xảy ra lỗi bất kỳ
            ViewBag.Movies = new SelectList(_context.Movies.Where(m => m.Status == 1), "MovieId", "Title", showtime.MovieId);
            ViewBag.Cinemas = new SelectList(_context.Cinemas.Where(c => c.Status == 1), "CinemaId", "Name", showtime.CinemaId);

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
