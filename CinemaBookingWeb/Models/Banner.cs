using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        public string ImageUrl { get; set; }

        // Thêm thuộc tính phân loại
        [Required]
        public string Category { get; set; } // Ví dụ: "Main" hoặc "Cinema"
    }
}
