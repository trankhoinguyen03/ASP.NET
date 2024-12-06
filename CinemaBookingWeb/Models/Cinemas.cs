using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 số và bắt đầu bằng số 0.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Thành phố không được để trống.")]
        public string City { get; set; } // Thành phố
        public byte Status { get; set; }
        [JsonIgnore]
        public ICollection<Showtimes>? Showtimes { get; set; }
    }
}
