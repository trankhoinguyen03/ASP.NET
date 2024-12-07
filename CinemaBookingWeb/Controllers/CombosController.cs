using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace CinemaBookingWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CombosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos");

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
        public async Task<IActionResult> Create(Combos combo, IFormFile? ImageFile)
        {

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var streamSave = ImageFile.OpenReadStream())
                {
                    using (var image = Image.FromStream(streamSave))
                    {
                        // Resize ảnh về kích thước chuẩn
                        var resizedImage = new Bitmap(image, new Size(4000, 2250));
                        var fileName = Path.GetFileName(ImageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos", fileName);


                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos")))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos"));
                        }

                        //using (var stream = new FileStream(filePath, FileMode.Create))
                        //{
                        //    ImageFile.CopyTo(stream);
                        //}
                        resizedImage.Save(filePath, image.RawFormat);
                        combo.ImageUrl = "/img/combos/" + fileName;
                    }
                }
            }
                           

            else if (string.IsNullOrEmpty(combo.ImageUrl))
            {
                // Nếu không có file và không có ImageUrl, thêm lỗi
                ModelState.AddModelError("ImageUrl", "Hãy tải lên một hình ảnh.");
                return View(combo);
            }

            if (ModelState.IsValid)
            {
                combo.Status = 1;
                _context.Combos.Add(combo);
                await _context.SaveChangesAsync();
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
            combo.Price = Math.Floor(combo.Price);
            return View(combo);
        }

        // POST: Combos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Combos combo, IFormFile? ImageFile)
        {
            if (id != combo.ComboId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var streamSave = ImageFile.OpenReadStream())
                    {
                        using (var image = Image.FromStream(streamSave))
                        {
                            // Resize ảnh về kích thước chuẩn
                            var resizedImage = new Bitmap(image, new Size(4000, 2250));
                            var fileName = Path.GetFileName(ImageFile.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos", fileName);


                            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos")))
                            {
                                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos"));
                            }

                            //using (var stream = new FileStream(filePath, FileMode.Create))
                            //{
                            //    ImageFile.CopyTo(stream);
                            //}
                            resizedImage.Save(filePath, image.RawFormat);
                            combo.ImageUrl = "/img/combos/" + fileName;
                        }
                    }
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