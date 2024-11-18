using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{

    public class ResetPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất phải có 6 ký tự.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }

}
