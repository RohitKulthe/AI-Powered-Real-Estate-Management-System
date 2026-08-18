using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models
{
    public class EmailOtp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string OTP { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryTime { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}