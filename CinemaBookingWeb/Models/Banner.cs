using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }
        public string ImageUrl { get; set; }
    }
}
