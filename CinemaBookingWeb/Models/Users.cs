using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime SignupDate { get; set; } = DateTime.Now;

        public string Role { get; set; } = "User";

        public byte Status { get; set; } = 1; // 1: Active, 0: Inactive
    }
}
