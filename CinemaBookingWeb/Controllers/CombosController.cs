﻿using CinemaBookingWeb.Data;
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
        public IActionResult CreateCreate(Combos combo, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                try
                {
                    // Tạo tên file
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    // Tạo đường dẫn lưu ảnh
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    var filePath = Path.Combine(imagesPath, fileName);

                    // Lưu ảnh vào thư mục "wwwroot/images"
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Gán giá trị cho trường ImageUrl
                    combo.ImageUrl = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Không thể tải lên hình ảnh: {ex.Message}");
                    return View(combo);
                }
            }
            else
            {
                // Nếu không có file tải lên, hiển thị lỗi
                ModelState.AddModelError("ImageUrl", "Hãy chọn hình ảnh.");
                return View(combo);
            }

            // Kiểm tra ModelState sau khi gán ImageUrl
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