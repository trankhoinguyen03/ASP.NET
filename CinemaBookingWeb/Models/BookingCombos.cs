using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class BookingCombos
    {
        [Key]
        public int BookingComboId { get; set; }
        public int BookingId { get; set; }
        public int ComboId { get; set; }
        public int Quantity { get; set; }
    }
}
