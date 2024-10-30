﻿using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class BookingDetails
    {
        [Key]
        public int BookingDetailId { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        public double Price { get; set; }
        public byte Status { get; set; }
    }
}
