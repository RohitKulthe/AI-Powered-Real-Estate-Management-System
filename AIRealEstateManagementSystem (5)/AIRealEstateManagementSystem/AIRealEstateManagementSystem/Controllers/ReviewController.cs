using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIRealEstateManagementSystem.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display Reviews of Logged-in Customer
        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var reviews = _context.Reviews
                                  .Include(r => r.Property)
                                  .Where(r => r.CustomerId == customerId.Value)
                                  .ToList();

            return View(reviews);
        }

        // GET
        public IActionResult Create(int propertyId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            return View(new Review
            {
                PropertyId = propertyId
            });
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            if (!ModelState.IsValid)
            {
                return View(review);
            }

            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            review.CustomerId = customerId.Value;
            review.ReviewDate = DateTime.Now;

            _context.Reviews.Add(review);
            _context.SaveChanges();

            TempData["Success"] = "Review submitted successfully.";

            return RedirectToAction("Details", "Property", new { id = review.PropertyId });
        }

        // Delete Review
        public IActionResult Delete(int id)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var review = _context.Reviews.FirstOrDefault(r =>
                r.ReviewId == id &&
                r.CustomerId == customerId.Value);

            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}