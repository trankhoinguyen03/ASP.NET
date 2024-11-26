using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Cinemas
    {
        [Key]
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "Tên rạp chiếu không được để trống.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vị trí không được để trống.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải chứa 10 chữ số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Thành phố không được để trống.")]
        public string City { get; set; } // Thành phố
        public byte Status { get; set; }
        public ICollection<Showtimes>? Showtimes { get; set; }
    }
}
