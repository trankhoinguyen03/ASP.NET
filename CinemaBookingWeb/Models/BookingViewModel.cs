using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaBookingWeb.Models
{
    public class BookingDetailViewModel
    {
        public Bookings Booking { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<BookingDetails> BookingDetails { get; set; }
        public List<BookingCombos> BookingCombos { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}
