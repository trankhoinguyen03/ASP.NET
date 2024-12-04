
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class tickets
    {
        [Key]
        public Movies movie { get; set; }
        public Cinemas cinema { get; set; }
        public Showtimes showtime { get; set; }
        public DateOnly? date { get; set; }
        public List<BookingDetails> seats { get; set; }
        public List<BookingCombos> BookingCombo { get; set; }
        public decimal totalPrice { get; set; }
        public decimal priceCombo { get; set; }
        public tickets() { }


    }
}
