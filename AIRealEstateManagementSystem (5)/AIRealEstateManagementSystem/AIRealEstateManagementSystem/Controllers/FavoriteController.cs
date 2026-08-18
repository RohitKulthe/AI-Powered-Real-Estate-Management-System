using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIRealEstateManagementSystem.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var favorites = _context.Favorites
                .Include(f => f.Property)
                .Where(f => f.CustomerId == customerId.Value)
                .ToList();

            return View(favorites);
        }

        public IActionResult Add(int propertyId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            bool exists = _context.Favorites.Any(f =>
                f.PropertyId == propertyId &&
                f.CustomerId == customerId.Value);

            if (!exists)
            {
                Favorite favorite = new Favorite
                {
                    PropertyId = propertyId,
                    CustomerId = customerId.Value
                };

                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var favorite = _context.Favorites.FirstOrDefault(f =>
                f.FavoriteId == id &&
                f.CustomerId == customerId.Value);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}