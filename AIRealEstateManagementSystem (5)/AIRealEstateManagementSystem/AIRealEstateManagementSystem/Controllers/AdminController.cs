using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}