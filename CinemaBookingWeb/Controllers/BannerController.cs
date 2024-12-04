using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Data; // Namespace chứa ApplicationDbContext
using CinemaBookingWeb.Models; // Namespace chứa model Banner
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BannerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Hiển thị trang Index
        public IActionResult Index()
        {
            // Lấy danh sách banner chính và banner rạp
            ViewBag.MainBanners = _context.Banner.Where(b => b.Category == "Main").ToList();
            ViewBag.CinemaBanners = _context.Banner.Where(b => b.Category == "Cinema").ToList();
            return View(new Banner());
        }

        [HttpPost]
        public IActionResult Create(IFormFile bannerImage, string category)
        {
            if (bannerImage != null && bannerImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/banners");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(bannerImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    bannerImage.CopyTo(fileStream);
                }

                Banner banner = new Banner
                {
                    ImageUrl = "/img/banners/" + uniqueFileName,
                    Category = category
                };

                _context.Banner.Add(banner);
                _context.SaveChanges();

                // Cập nhật lại ViewBag
                ViewBag.MainBanners = _context.Banner.Where(b => b.Category == "Main").ToList();
                ViewBag.CinemaBanners = _context.Banner.Where(b => b.Category == "Cinema").ToList();

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("ImageUrl", "Vui lòng chọn một file ảnh hợp lệ.");
            return View("Index");
        }



        // Xóa banner
        public IActionResult Delete(int id)
        {
            var banner = _context.Banner.FirstOrDefault(b => b.BannerId == id);
            if (banner != null)
            {
                // Xóa file ảnh trong wwwroot
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, banner.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Xóa banner trong cơ sở dữ liệu
                _context.Banner.Remove(banner);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
