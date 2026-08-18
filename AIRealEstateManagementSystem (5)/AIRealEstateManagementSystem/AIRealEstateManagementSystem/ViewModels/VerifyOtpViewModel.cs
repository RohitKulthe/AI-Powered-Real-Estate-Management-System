using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.ViewModels
{
    public class VerifyOtpViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "OTP")]
        public string OTP { get; set; } = string.Empty;
    }
}