using CinemaBookingWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Cinemas> Cinemas { get; set; }
        public DbSet<Showtimes> Showtimes { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Combos> Combos { get; set; }
        public DbSet<BookingCombos> BookingCombos { get; set; }
    }
}
