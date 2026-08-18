using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PropertyController(ApplicationDbContext context,
                                  IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Display all available properties
        public IActionResult Index(string? city, string? propertyType, decimal? minPrice, decimal? maxPrice, int? bedrooms)
        {
            var properties = _context.Properties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(city))
                properties = properties.Where(p => p.City.Contains(city));

            if (!string.IsNullOrWhiteSpace(propertyType))
                properties = properties.Where(p => p.PropertyType.Contains(propertyType));

            if (minPrice.HasValue)
                properties = properties.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                properties = properties.Where(p => p.Price <= maxPrice.Value);

            if (bedrooms.HasValue)
                properties = properties.Where(p => p.Bedrooms == bedrooms.Value);

            properties = properties.Where(p => p.Status == "Available");

            return View(properties.ToList());
        }

        // GET: Create
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Property property)
        {
            if (ModelState.IsValid)
            {
                if (property.ImageFile != null)
                {
                    string folder = Path.Combine(_environment.WebRootPath, "images");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(property.ImageFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using FileStream stream = new(filePath, FileMode.Create);
                    property.ImageFile.CopyTo(stream);

                    property.ImagePath = "/images/" + fileName;
                }

                _context.Properties.Add(property);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }

        // GET: Edit
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Edit(int id)
        {
            var property = _context.Properties.Find(id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        // POST : Edit
        // POST: Edit
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Property property)
        {
            ModelState.Remove(nameof(Property.ImagePath));
            ModelState.Remove(nameof(Property.ImageFile));

            if (ModelState.IsValid)
            {
                var oldProperty = _context.Properties.Find(property.PropertyId);

                if (oldProperty == null)
                    return NotFound();

                oldProperty.PropertyName = property.PropertyName;
                oldProperty.City = property.City;
                oldProperty.Location = property.Location;
                oldProperty.Price = property.Price;
                oldProperty.Bedrooms = property.Bedrooms;
                oldProperty.Bathrooms = property.Bathrooms;
                oldProperty.Area = property.Area;
                oldProperty.PropertyType = property.PropertyType;
                oldProperty.Status = property.Status;
                oldProperty.Latitude = property.Latitude;
                oldProperty.Longitude = property.Longitude;
                oldProperty.Description = property.Description;

                if (property.ImageFile != null)
                {
                    string folder = Path.Combine(_environment.WebRootPath, "images");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(property.ImageFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using FileStream stream = new(filePath, FileMode.Create);
                    property.ImageFile.CopyTo(stream);

                    oldProperty.ImagePath = "/images/" + fileName;
                }

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }

        // GET: Delete
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Delete(int id)
        {
            var property = _context.Properties.Find(id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Seller")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var property = _context.Properties.Find(id);

            if (property != null)
            {
                _context.Properties.Remove(property);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public IActionResult Details(int id)
        {
            var property = _context.Properties.FirstOrDefault(p => p.PropertyId == id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        // GET: Search - Filter properties from home page
        public IActionResult Search(string location, string type, string priceRange, int? bedrooms)
        {
            var properties = _context.Properties.AsQueryable();

            // Filter by location (city or location field)
            if (!string.IsNullOrWhiteSpace(location))
            {
                properties = properties.Where(p => 
                    p.Location.Contains(location) || p.City.Contains(location));
            }

            // Filter by property type
            if (!string.IsNullOrWhiteSpace(type) && type != "All Types")
            {
                properties = properties.Where(p => p.PropertyType.Contains(type));
            }

            // Filter by price range
            if (!string.IsNullOrWhiteSpace(priceRange))
            {
                properties = ApplyPriceRangeFilter(properties, priceRange);
            }

            // Filter by bedrooms
            if (bedrooms.HasValue && bedrooms.Value > 0)
            {
                properties = properties.Where(p => p.Bedrooms == bedrooms.Value);
            }

            // Only show available properties
            properties = properties.Where(p => p.Status == "Available");

            return View("Index", properties.ToList());
        }

        // Helper method to apply price range filter
        private IQueryable<Property> ApplyPriceRangeFilter(IQueryable<Property> query, string priceRange)
        {
            return priceRange switch
            {
                "Under 50 L" => query.Where(p => p.Price < 5000000),
                "50L - 1Cr" => query.Where(p => p.Price >= 5000000 && p.Price < 10000000),
                "1Cr - 2Cr" => query.Where(p => p.Price >= 10000000 && p.Price < 20000000),
                "Above 2Cr" => query.Where(p => p.Price >= 20000000),
                _ => query
            };
        }
    }
}