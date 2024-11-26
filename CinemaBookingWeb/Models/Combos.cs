using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingWeb.Models
{
    public class Combos
    {
        [Key]
        public int ComboId { get; set; }

        [Required(ErrorMessage = "Tên combo không được để trống")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]

        [Required]
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public byte Status { get; set; }
    }
}
