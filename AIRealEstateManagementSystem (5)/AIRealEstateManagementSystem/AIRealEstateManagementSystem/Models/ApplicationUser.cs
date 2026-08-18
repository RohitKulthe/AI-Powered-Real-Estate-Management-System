using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}