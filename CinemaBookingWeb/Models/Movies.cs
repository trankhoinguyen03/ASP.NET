using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Movies
    {
        [Key]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Tên phim không được để trống.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Không được để trống.")]
        public string Rating { get; set; }
        public DateOnly ReleaseDate { get; set; }

        [Required(ErrorMessage = "Thể loại không được để trống.")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Ngôn ngữ không được để trống.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "TrailerUrl không được để trống.")]
        public string TrailerUrl { get; set; }

        [Required(ErrorMessage = "Bạn cần chọn một ảnh.")]
        [RegularExpression(@".*\.(jpg|jpeg|png)$", ErrorMessage = "Ảnh phải có định dạng jpg, jpeg hoặc png.")]
        public string ImageUrl { get; set; }
        public byte Status { get; set; }
        public ICollection<Showtimes>? Showtimes { get; set; }
    }
}
