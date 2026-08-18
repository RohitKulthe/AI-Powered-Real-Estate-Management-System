using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AIRealEstateManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show logged-in customer's bookings
        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var bookings = _context.Bookings
                                   .Where(b => b.CustomerId == customerId.Value)
                                   .ToList();

            return View(bookings);
        }

        // Booking Form
        public IActionResult Create(int propertyId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            Booking booking = new Booking
            {
                PropertyId = propertyId
            };

            return View(booking);
        }

        // Save Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            booking.CustomerId = customerId.Value;
            booking.Status = "Pending";
            booking.CreatedDate = DateTime.Now;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            TempData["Success"] = "Booking request submitted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}