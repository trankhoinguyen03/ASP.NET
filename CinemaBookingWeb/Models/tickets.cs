using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class tickets
    {
        [Key]
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public int ShowtimeId { get; set; }
        public int viewTemp {  get; set; }
        public DateOnly? date { get; set; }  
        public List<BookingDetails> seats { get; set; }
        public List<BookingCombos> BookingCombo { get; set; } 
        public decimal totalPrice { get; set; }
        public tickets() { }


    }
}
