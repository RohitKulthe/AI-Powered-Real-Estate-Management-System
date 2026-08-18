using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AIRealEstateManagementSystem.Models;

namespace AIRealEstateManagementSystem.Controllers
{
    public class DebugController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DebugController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Display current user's info (authentication testing)
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Content("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = User.Claims.ToList();

            var info = $@"
User: {user.UserName}
Email: {user.Email}
Email Confirmed: {user.EmailConfirmed}
Roles: {string.Join(", ", roles)}

Claims:
{string.Join("<br/>", claims.Select(c => $"{c.Type}: {c.Value}"))}
";

            return Content(info, "text/html");
        }

        // List all users and their roles (admin only for safety)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers()
        {
            var users = _userManager.Users.ToList();
            var html = "<h1>All Users</h1><table border='1'><tr><th>Username</th><th>Email</th><th>Roles</th><th>Email Confirmed</th></tr>";

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                html += $"<tr><td>{user.UserName}</td><td>{user.Email}</td><td>{string.Join(", ", roles)}</td><td>{user.EmailConfirmed}</td></tr>";
            }

            html += "</table>";
            return Content(html, "text/html");
        }

        // Manually assign Seller role (admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignSellerRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Content($"User {email} not found");

            // Ensure Seller role exists
            if (!await _roleManager.RoleExistsAsync("Seller"))
                await _roleManager.CreateAsync(new IdentityRole("Seller"));

            // Remove Customer role and add Seller role
            await _userManager.RemoveFromRoleAsync(user, "Customer");
            await _userManager.AddToRoleAsync(user, "Seller");

            return Content($"User {email} has been assigned the Seller role");
        }
    }
}
