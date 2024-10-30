using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateTime BookingDate { get; set; }
        public double TotalPrice { get; set; }
        public byte Status { get; set; }
    }
}
