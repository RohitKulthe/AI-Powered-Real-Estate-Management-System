using AIRealEstateManagementSystem.Models;
using AIRealEstateManagementSystem.Services.Authentication;
using AIRealEstateManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AIRealEstateManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        // ===========================
        // Register
        // ===========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model);

            if (result.Succeeded)
            {
                TempData["Email"] = model.Email;
                return RedirectToAction("VerifyOtp");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ===========================
        // Verify OTP
        // ===========================

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool result = await _authService.VerifyOtpAsync(model.Email, model.OTP);

            if (result)
            {
                TempData["Success"] = "Email verified successfully. Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Invalid or expired OTP.");
            return View(model);
        }

        // ===========================
        // Login
        // ===========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.LoginAsync(model);

            if (user != null)
            {
                bool isAdmin = await _authService.IsAdminAsync(user);

                if (isAdmin)
                {
                    return RedirectToAction("Index", "Property");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid credentials or email not verified.");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ===========================
        // Access Denied
        // ===========================

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}