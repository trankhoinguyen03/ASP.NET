using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Showtimes
    {
        [Key]
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public double Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Hall { get; set; }
    }
}
