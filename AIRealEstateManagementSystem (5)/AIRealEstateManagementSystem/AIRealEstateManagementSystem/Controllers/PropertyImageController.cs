using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyImageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PropertyImageController(ApplicationDbContext context,
                                       IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Create(int propertyId)
        {
            PropertyImage image = new PropertyImage
            {
                PropertyId = propertyId
            };

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PropertyImage image)
        {
            if (ModelState.IsValid)
            {
                if (image.ImageFile != null)
                {
                    string folder = Path.Combine(_environment.WebRootPath, "images");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string fileName = Guid.NewGuid() + Path.GetExtension(image.ImageFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using FileStream stream = new(filePath, FileMode.Create);
                    image.ImageFile.CopyTo(stream);

                    image.ImagePath = "/images/" + fileName;
                }

                _context.PropertyImages.Add(image);
                _context.SaveChanges();

                return RedirectToAction("Details", "Property", new { id = image.PropertyId });
            }

            return View(image);
        }
    }
}