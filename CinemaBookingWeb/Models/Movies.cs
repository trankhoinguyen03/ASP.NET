using CinemaBookingWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Movies
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Rating { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public string TrailerUrl { get; set; }
        public string ImageUrl { get; set; }
        public byte Status { get; set; } = 1;
    }
}

