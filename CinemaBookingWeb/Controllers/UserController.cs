using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CinemaBookingWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users/Index
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Users newUser)
        {
/*            if (ModelState.IsValid)*/
            {
                // Kiểm tra trùng lặp Username
                if (_context.Users.Any(u => u.UserName == newUser.UserName))
                {
                    ModelState.AddModelError("UserName", "Tên người dùng đã tồn tại.");
                }

                // Kiểm tra trùng lặp Email
                if (_context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                }

                // Kiểm tra trùng lặp Phone
                if (_context.Users.Any(u => u.Phone == newUser.Phone))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại.");
                }

                // Nếu có lỗi, trả về View
                if (!ModelState.IsValid)
                {
                    return View(newUser);
                }

                // Thêm người dùng mới
                newUser.SignupDate = DateTime.Now;
                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

/*            return View(newUser);*/
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Users updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return BadRequest("ID không khớp với người dùng.");
            }

            /*if (ModelState.IsValid)*/
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                // Kiểm tra trùng lặp Email (trừ người dùng hiện tại)
                if (_context.Users.Any(u => u.Email == updatedUser.Email && u.UserId != id))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(updatedUser);
                }

                // Kiểm tra trùng lặp Phone (trừ người dùng hiện tại)
                if (_context.Users.Any(u => u.Phone == updatedUser.Phone && u.UserId != id))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại.");
                    return View(updatedUser);
                }

                // Cập nhật thông tin
                user.UserName = updatedUser.UserName;
                user.Password = updatedUser.Password;
                user.Email = updatedUser.Email;
                user.Phone = updatedUser.Phone;
                user.SignupDate = updatedUser.SignupDate;
                user.Role = updatedUser.Role;
                user.Status = updatedUser.Status;
                
                // Lưu thay đổi
                _context.Update(user);
                _context.SaveChanges();


                return RedirectToAction("Index");
            }

            /*return RedirectToAction("Index");*/
        }
    }
}
