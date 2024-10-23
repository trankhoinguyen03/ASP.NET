using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateOnly BookingDate { get; set; }
        public double TotalPrice { get; set; }
    }
}
