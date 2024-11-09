namespace CinemaBookingWeb.Models
{
    public class OverviewModel
    {
        public List<DayStatistic_Count> NewUsersByDay { get; set; }
        public List<MonthStatistic_Count> NewUsersByMonth { get; set; }
        public List<DayStatistic_Count> TotalTicketsByDay { get; set; }
        public List<MonthStatistic_Count> TotalTicketsByMonth { get; set; }
        public List<DayStatistic_Revenue> TotalRevenueByDay { get; set; }
        public List<MonthStatistic_Revenue> TotalRevenueByMonth { get; set; }
        public List<Statistic_Movies_Cinemas> MoviesRevenue { get; set; }
        public List<Statistic_Movies_Cinemas> CinemasRevenue { get; set; }
    }

    public class DayStatistic_Count
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class MonthStatistic_Count
    {
        public DateTime Month { get; set; }
        public int Count { get; set; }
    }
    public class DayStatistic_Revenue
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthStatistic_Revenue
    {
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
    }
    public class Statistic_Movies_Cinemas
    {
        public string Name { get; set; }
        public decimal Revenue { get; set; }
    }
}
