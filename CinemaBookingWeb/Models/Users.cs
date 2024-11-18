using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingWeb.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
        [MinLength(6, ErrorMessage = "Tài khoản phải có ít nhất 6 ký tự.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [NotMapped] // Thuộc tính này sẽ không được lưu vào cơ sở dữ liệu
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu xác nhận.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 số và bắt đầu bằng số 0.")]
        public string Phone { get; set; }

        public DateTime SignupDate { get; set; }

        public string Role { get; set; } = "User";

        public byte Status { get; set; }
    }
}
