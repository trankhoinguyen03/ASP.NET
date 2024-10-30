using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime SignupDate { get; set; }
        public string Role { get; set; }
        public byte Status { get; set; }
    }
}
