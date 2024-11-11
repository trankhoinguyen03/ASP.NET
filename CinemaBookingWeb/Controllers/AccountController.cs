using CinemaBookingWeb.Data;
using CinemaBookingWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaBookingWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Users user)
        {
            if (_context.Users.Any(u => u.UserName == user.UserName))
            {
                ModelState.AddModelError("UserName", "UserName đã tồn tại, vui lòng chọn tên khác.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.SignupDate = DateTime.Now;
                user.Status = 1;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Log ModelState errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(user);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, bool rememberMe)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);

            if (user != null)
            {
                // Tạo các claim
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Thiết lập thời gian lưu cookie dựa trên giá trị "rememberMe"
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTime.UtcNow.AddDays(14) : DateTime.UtcNow.AddHours(1)
                };

                // Đăng nhập người dùng
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Điều kiện chuyển hướng
                if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }
            }

            // Thông báo lỗi đăng nhập
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        // Trang quên mật khẩu (GET)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Xử lý quên mật khẩu (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email, string userName)
        {
            // Kiểm tra xem email có tồn tại trong hệ thống không
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.UserName == userName);
            if (user == null)
            {
                ViewBag.Error = "Tài khoản hoặc Email không đúng.";
                return View();
            }

            // Chuyển đến trang để người dùng thay đổi mật khẩu
return RedirectToAction("ResetPassword", new { email = user.Email, userName = user.UserName });

        }

        // Trang đặt lại mật khẩu (GET)
        [HttpGet]
        public IActionResult ResetPassword(string email, string userName)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }
            // Kiểm tra name có tồn tại trong hệ thống không
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Tài khoản hoặc Email không đúng.";
                return RedirectToAction("ForgotPassword");
            }


            return View(new ResetPasswordViewModel { UserName = userName, Email = email });
        }


        // Xử lý đặt lại mật khẩu (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mật khẩu mới có đủ mạnh hay không
                if (model.NewPassword.Length < 6) // Ví dụ: mật khẩu tối thiểu 6 ký tự
                {
                    ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                    return View(model);
                }

                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Email == model.Email); 
                if (user != null)
                {
                    user.Password = model.NewPassword;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Mật khẩu của bạn đã được thay đổi thành công.";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Error = "Email không tồn tại.";
                    return View();
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
