using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Seats
    {
        [Key]
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; }
        public byte Status { get; set; }
    }
}
