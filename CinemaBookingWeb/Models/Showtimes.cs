using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Showtimes
    {
        [Key]
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Hall { get; set; }
        public byte Status { get; set; }
        public Cinemas? Cinema { get; set; }
        public Movies? Movie { get; set; }
    }
}
