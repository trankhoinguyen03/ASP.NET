﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CinemaBookingWeb.Models
{
    public class Showtimes
    {
        [Key]
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]

        [Required]
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Hall { get; set; }
        public byte Status { get; set; }
        [JsonIgnore]
        public Cinemas? Cinema { get; set; }
        [JsonIgnore]
        public Movies? Movie { get; set; }
    }
}
