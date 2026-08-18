using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AIRealEstateManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardViewModel dashboard = new DashboardViewModel();

            // Dashboard Cards
            dashboard.TotalProperties = _context.Properties.Count();
            dashboard.AvailableProperties = _context.Properties.Count(x => x.Status == "Available");
            dashboard.SoldProperties = _context.Properties.Count(x => x.Status == "Sold");
            dashboard.RentedProperties = _context.Properties.Count(x => x.Status == "Rented");

            dashboard.TotalUsers = _context.Users.Count();
            dashboard.TotalBookings = _context.Bookings.Count();
            dashboard.TotalReviews = _context.Reviews.Count();

            // Bar Chart - Properties by City
            dashboard.CityPropertyCounts = _context.Properties
                .GroupBy(p => p.City)
                .Select(g => new CityPropertyCount
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var bookingData = _context.Bookings
    .GroupBy(b => b.CreatedDate.Month)
    .Select(g => new
    {
        MonthNumber = g.Key,
        Count = g.Count()
    })
    .OrderBy(x => x.MonthNumber)
    .ToList();

            dashboard.MonthlyBookingCounts = bookingData
                .Select(x => new MonthlyBookingCount
                {
                    Month = System.Globalization.CultureInfo.CurrentCulture
                        .DateTimeFormat.GetMonthName(x.MonthNumber),
                    Count = x.Count
                })
                .ToList();

            return View(dashboard);
        }
    }
}