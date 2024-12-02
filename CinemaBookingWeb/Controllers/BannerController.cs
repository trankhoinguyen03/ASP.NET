using Microsoft.AspNetCore.Mvc;
using CinemaBookingWeb.Data; // Namespace chứa ApplicationDbContext
using CinemaBookingWeb.Models; // Namespace chứa model Banner
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CinemaBookingWeb.Controllers
{
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
            var banners = _context.Banner.ToList();
            ViewBag.Banners = banners;
            return View(new Banner());
        }

        // Xử lý thêm mới banner
        [HttpPost]
        public IActionResult Create(IFormFile bannerImage)
        {
            if (bannerImage != null && bannerImage.Length > 0)
            {
                // Tạo đường dẫn lưu file
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/banners");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(bannerImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vào wwwroot/uploads/banners
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    bannerImage.CopyTo(fileStream);
                }

                // Lưu thông tin vào cơ sở dữ liệu
                Banner banner = new Banner
                {
                    ImageUrl =  uniqueFileName
                };

                _context.Banner.Add(banner);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu không có file, hiển thị lại trang
            ViewBag.Banners = _context.Banner.ToList();
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
