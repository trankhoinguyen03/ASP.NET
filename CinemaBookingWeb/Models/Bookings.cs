using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingWeb.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public byte Status { get; set; }
        public ICollection<BookingDetails>? bookingDetails { get; set; }

        [ForeignKey("UserId")]
        public Users Users { get; set; }
    }
}
