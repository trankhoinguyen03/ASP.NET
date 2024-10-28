namespace CinemaBookingWeb.Models
{
    public class OverviewModel
    {
        public int NewUsers { get; set; }
        public int TotalTickets { get; set; }
        public decimal MoviesRevenue { get; set; }
        public decimal CinemasRevenue { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }
}
