namespace AIRealEstateManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalProperties { get; set; }

        public int AvailableProperties { get; set; }

        public int SoldProperties { get; set; }

        public int RentedProperties { get; set; }

        public int TotalUsers { get; set; }

        public int TotalBookings { get; set; }

        public int TotalReviews { get; set; }

        public List<CityPropertyCount> CityPropertyCounts { get; set; } = new();

        public List<MonthlyBookingCount> MonthlyBookingCounts { get; set; } = new();
    }
}