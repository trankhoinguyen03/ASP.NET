using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Showtimes
    {
        [Key]
        public int ShowtimeId { get; set; }

        [Required(ErrorMessage = "MovieId là bắt buộc.")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "CinemaId là bắt buộc.")]
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "Giá vé là bắt buộc.")]
        [Range(1, 1000000, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc.")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Tên phòng chiếu là bắt buộc.")]
        [StringLength(60, ErrorMessage = "Tên phòng chiếu không được dài quá 60 ký tự.")]
        public string Hall { get; set; }

        public byte Status { get; set; }


        public Movies Movie { get; set; }
        public Cinemas Cinema { get; set; }
    }
}
