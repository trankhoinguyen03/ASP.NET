namespace CinemaBookingWeb.Models
{
    public class OverviewModel
    {
        public List<DayStatistic> NewUsersByDay { get; set; }
        public List<MonthStatistic> NewUsersByMonth { get; set; }
        public List<DayStatistic> TotalTicketsByDay { get; set; }
        public List<MonthStatistic> NewTicketsByMonth { get; set; }

    }

    public class DayStatistic
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class MonthStatistic
    {
        public DateTime Month { get; set; }
        public int Count { get; set; }
    }
}
