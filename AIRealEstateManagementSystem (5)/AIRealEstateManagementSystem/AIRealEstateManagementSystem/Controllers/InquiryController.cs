using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    public class InquiryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show all inquiries (Admin/Seller can use this later)
        public IActionResult Index()
        {
            var inquiries = _context.Inquiries
                                    .Select(i => new Inquiry
                                    {
                                        InquiryId = i.InquiryId,
                                        PropertyId = i.PropertyId,
                                        Property = _context.Properties.FirstOrDefault(p => p.PropertyId == i.PropertyId),
                                        Name = i.Name,
                                        Email = i.Email,
                                        Phone = i.Phone,
                                        Message = i.Message,
                                        InquiryDate = i.InquiryDate,
                                        Status = i.Status
                                    })
                                    .ToList();

            return View(inquiries);
        }

        // GET
        public IActionResult Create(int propertyId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            Inquiry inquiry = new Inquiry
            {
                PropertyId = propertyId,
                Name = customer.FullName,
                Email = customer.Email,
                Phone = customer.MobileNo
            };

            return View(inquiry);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquiry inquiry)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Customer");
            }

            if (ModelState.IsValid)
            {
                inquiry.Name = customer.FullName;
                inquiry.Email = customer.Email;
                inquiry.Phone = customer.MobileNo;
                inquiry.InquiryDate = DateTime.Now;

                _context.Inquiries.Add(inquiry);
                _context.SaveChanges();

                TempData["Success"] = "Your inquiry has been sent successfully.";

                return RedirectToAction("Details", "Property", new { id = inquiry.PropertyId });
            }

            return View(inquiry);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var inquiry = _context.Inquiries.Find(id);

            if (inquiry == null)
            {
                return NotFound();
            }

            return View(inquiry);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Inquiries.Update(inquiry);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(inquiry);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var inquiry = _context.Inquiries
                                  .FirstOrDefault(i => i.InquiryId == id);

            if (inquiry == null)
            {
                return NotFound();
            }

            return View(inquiry);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var inquiry = _context.Inquiries.Find(id);

            if (inquiry != null)
            {
                _context.Inquiries.Remove(inquiry);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}