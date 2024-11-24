﻿using System.ComponentModel.DataAnnotations;

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
        public Users Users { get; set; }
    }
}
