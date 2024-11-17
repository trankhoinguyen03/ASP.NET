using System.ComponentModel.DataAnnotations;

namespace CinemaBookingWeb.Models
{
    public class Cinemas
    {
        [Key]
        public int CinemaId { get; set; }
        public string Name {  get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public byte Status { get; set; }

        public ICollection<Showtimes> Showtimes { get; set; }
    }
}
