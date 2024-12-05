using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingWeb.Controllers
{
    public class CombosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");

        public CombosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Combos
        public IActionResult Index(string searchString)
        {
            var combos = _context.Combos.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                combos = combos.Where(c => c.Name.Contains(searchString) || c.Description.Contains(searchString));
            }

            return View(combos.ToList());
        }

        // GET: Combos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Combos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Combos combo, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                try
                {
                    // Tạo tên file và đường dẫn để lưu
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

                    // Đảm bảo thư mục hình ảnh tồn tại
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    // Lưu hình ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Cập nhật tên file vào trường ImageUrl của combo
                    combo.ImageUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Không thể tải lên hình ảnh: {ex.Message}");
                    return View(combo);
                }
            }
            else if (!string.IsNullOrEmpty(combo.ImageUrl))
            {
                // Nếu không có hình ảnh mới, nhưng đã có tên hình ảnh từ trước, giữ nguyên tên file
                // (giả sử combo.ImageUrl đã chứa tên file từ trước hoặc bạn muốn sử dụng một hình ảnh có sẵn)
                var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", combo.ImageUrl);

                // Kiểm tra xem file đã tồn tại trong thư mục hay chưa
                if (System.IO.File.Exists(existingFilePath))
                {
                    // Nếu hình ảnh đã có, bạn chỉ cần giữ lại tên file
                    combo.ImageUrl = combo.ImageUrl; // Không thay đổi gì nếu đã có tên file
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Hình ảnh không tồn tại trong thư mục.");
                    return View(combo);
                }
            }

            else
            {

                ModelState.AddModelError("ImageUrl", "Hãy chọn hình ảnh.");
                return View(combo);
            }

            // Kiểm tra ModelState trước khi lưu vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                _context.Combos.Add(combo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(combo);
        }


            // GET: Combos/Edit/5
            public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = _context.Combos.Find(id);
            if (combo == null)
            {
                return NotFound();
            }
            return View(combo);
        }

        // POST: Combos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Combos combo, IFormFile ImageFile)
        {
            if (id != combo.ComboId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Nếu có ảnh mới, xử lý ảnh mới, nếu không giữ lại tên ảnh cũ
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(_imageFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    combo.ImageUrl = fileName;
                }

                // Nếu không chọn ảnh mới, giữ lại tên ảnh cũ
                var existingCombo = _context.Combos.AsNoTracking().FirstOrDefault(c => c.ComboId == id);
                if (existingCombo != null && string.IsNullOrEmpty(combo.ImageUrl))
                {
                    combo.ImageUrl = existingCombo.ImageUrl;  // Giữ tên ảnh cũ
                }

                _context.Update(combo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(combo);
        
        }

        // GET: Combos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = _context.Combos
                .FirstOrDefault(m => m.ComboId == id);
            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        // POST: Combos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var combo = _context.Combos.Find(id);

            _context.Combos.Remove(combo);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CombosExists(int id)
        {
            return _context.Combos.Any(e => e.ComboId == id);
        }
    }
}