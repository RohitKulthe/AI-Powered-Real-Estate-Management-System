using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty ;

    [Required]
    public string MobileNo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User";

    public ICollection<Booking>? Bookings { get; set; }

    public ICollection<Favorite>? Favorites { get; set; }

    public ICollection<Review>? Reviews { get; set; }
}

