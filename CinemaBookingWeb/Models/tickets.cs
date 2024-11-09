using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class tickets
    {
        [Key]
        public int? MovieId { get; set; }
        public int CinemaId { get; set; }
        public int? ShowtimeId { get; set; }
        public List<int>? seats { get; set; }
        public List<BookingCombos>? BookingCombo { get; set; } 

        public tickets() { }


    }
}
