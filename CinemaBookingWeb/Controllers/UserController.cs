using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

		public IActionResult Index()
		{
			var users = _context.Users.ToList();
			return View(users);
		}

        // GET: User/Edit/5
        public IActionResult Edit(int id)
        {
            var users = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Users updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                // Cập nhật các thuộc tính
                user.UserName = updatedUser.UserName;
                user.Password = updatedUser.Password;
                user.Email = updatedUser.Email;
                user.Phone = updatedUser.Phone;
                user.SignupDate = updatedUser.SignupDate;
                user.Role = updatedUser.Role;
                user.Status = updatedUser.Status; // Lưu giá trị 1 hoặc 0

                // Cập nhật thông tin vào cơ sở dữ liệu
                _context.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "User");
            }

            return View(updatedUser);
        }
        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Users newUser)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra trùng lặp Username
				if (_context.Users.Any(u => u.UserName == newUser.UserName))
				{
					ModelState.AddModelError("Username", "Tên người dùng đã tồn tại.");
				}

				// Kiểm tra trùng lặp Email
				if (_context.Users.Any(u => u.Email == newUser.Email))
				{
					ModelState.AddModelError("Email", "Email đã tồn tại.");
				}
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

				return RedirectToAction(nameof(Index));
			}

			return View(newUser);
		}





	}
}
