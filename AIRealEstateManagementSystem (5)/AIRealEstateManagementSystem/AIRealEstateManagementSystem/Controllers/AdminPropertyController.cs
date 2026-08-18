using AIRealEstateManagementSystem.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPropertyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var properties = _context.Properties.ToList();
            return View(properties);
        }
    }
}