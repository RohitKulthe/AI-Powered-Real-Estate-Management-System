using AIRealEstateManagementSystem.Dal;
using AIRealEstateManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    [Route("Chat")]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new ChatResponse
                {
                    Response = "Please enter a message."
                });
            }

            string message = request.Message.Trim().ToLower();

            // Search by City
            var cities = _context.Properties
                .Select(p => p.City)
                .Distinct()
                .ToList();

            foreach (var city in cities)
            {
                if (!string.IsNullOrWhiteSpace(city) &&
                    message.Contains(city.Trim().ToLower()))
                {
                    return Json(SearchPropertyByCity(city));
                }
            }

            // Search by Property Type
            var propertyTypes = _context.Properties
                .Select(p => p.PropertyType)
                .Distinct()
                .ToList();

            foreach (var type in propertyTypes)
            {
                if (!string.IsNullOrWhiteSpace(type) &&
                    message.Contains(type.Trim().ToLower()))
                {
                    return Json(SearchPropertyByType(type));
                }
            }

            if (message.Contains("available"))
            {
                return Json(SearchAvailableProperties());
            }

            if (message.Contains("hello") || message.Contains("hi"))
            {
                return Json(new ChatResponse
                {
                    Response = "👋 Hello! Welcome to AI Real Estate. How can I help you today?"
                });
            }

            if (message.Contains("book"))
            {
                return Json(new ChatResponse
                {
                    Response = "📅 To book a property, open the Property Details page and click the 'Book Now' button."
                });
            }

            if (message.Contains("favorite") || message.Contains("wishlist"))
            {
                return Json(new ChatResponse
                {
                    Response = "❤️ You can add any property to your Favorites from the Property Details page."
                });
            }

            if (message.Contains("contact"))
            {
                return Json(new ChatResponse
                {
                    Response = "📞 You can contact the owner from the Property Details page."
                });
            }

            if (message.Contains("property") || message.Contains("properties"))
            {
                int count = _context.Properties.Count(p => p.Status == "Available");

                return Json(new ChatResponse
                {
                    Response = $"🏠 We currently have {count} available properties."
                });
            }

            return Json(new ChatResponse
            {
                Response = "🤖 Sorry, I didn't understand your question. Please ask about properties, booking, favorites or contact."
            });
        }

        private ChatResponse SearchPropertyByCity(string city)
        {
            var properties = _context.Properties
                .Where(p =>
                    p.City.Trim().ToLower() == city.Trim().ToLower() &&
                    p.Status.Trim().ToLower() == "available")
                .Take(5)
                .ToList();

            if (!properties.Any())
            {
                return new ChatResponse
                {
                    Response = $"❌ No available properties found in {city}."
                };
            }

            string response = $"🏠 Available Properties in {city}\n\n";

            foreach (var property in properties)
            {
                response += $"• {property.PropertyName}\n";
                response += $"📍 {property.Location}\n";
                response += $"💰 ₹{property.Price:N0}\n";
                response += $"🛏 {property.Bedrooms} BHK\n\n";
            }

            return new ChatResponse
            {
                Response = response
            };
        }

        private ChatResponse SearchPropertyByType(string propertyType)
        {
            var properties = _context.Properties
                .Where(p =>
                    p.PropertyType.Trim().ToLower() == propertyType.Trim().ToLower() &&
                    p.Status.Trim().ToLower() == "available")
                .Take(5)
                .ToList();

            if (!properties.Any())
            {
                return new ChatResponse
                {
                    Response = $"❌ No available {propertyType} properties found."
                };
            }

            string response = $"🏠 Available {propertyType} Properties\n\n";

            foreach (var property in properties)
            {
                response += $"• {property.PropertyName}\n";
                response += $"📍 {property.City}, {property.Location}\n";
                response += $"💰 ₹{property.Price:N0}\n";
                response += $"🛏 {property.Bedrooms} BHK\n\n";
            }

            return new ChatResponse
            {
                Response = response
            };
        }

        private ChatResponse SearchAvailableProperties()
        {
            var properties = _context.Properties
                .Where(p => p.Status.Trim().ToLower() == "available")
                .Take(5)
                .ToList();

            if (!properties.Any())
            {
                return new ChatResponse
                {
                    Response = "❌ No available properties found."
                };
            }

            string response = "🏠 Available Properties\n\n";

            foreach (var property in properties)
            {
                response += $"• {property.PropertyName}\n";
                response += $"📍 {property.City}\n";
                response += $"💰 ₹{property.Price:N0}\n\n";
            }

            return new ChatResponse
            {
                Response = response
            };
        }
    }
}