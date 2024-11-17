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

        public CombosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Combos
        public IActionResult Index()
        {
            var combos = _context.Combos.ToList(); // Lấy danh sách combo
            return View(combos);
        }

        // GET: Combos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Combos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Combos combo)
        {
            if (ModelState.IsValid)
            {
                _context.Combos.Add(combo);
                _context.SaveChanges(); // Lưu combo vào cơ sở dữ liệu
                return RedirectToAction(nameof(Index));
            }
            return View(combo);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Combos combo)
        {
            if (id != combo.ComboId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(combo);
                _context.SaveChanges(); // Cập nhật combo trong cơ sở dữ liệu
                return RedirectToAction(nameof(Index));
            }
            return View(combo);
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = _context.Combos.FirstOrDefault(m => m.ComboId == id);
            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var combo = _context.Combos.Find(id);
            if (combo != null)
            {
                _context.Combos.Remove(combo);
                _context.SaveChanges(); // Xóa combo khỏi cơ sở dữ liệu
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CombosExists(int id)
        {
            return _context.Combos.Any(e => e.ComboId == id);
        }
    }
}