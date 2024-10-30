using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Combos
    {
        [Key]
        public int ComboId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public byte Status { get; set; }
    }
}
