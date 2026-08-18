using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIRealEstateManagementSystem.Models;

public class Property
{
    [Key]
    public int PropertyId { get; set; }

    [Required]
    public string PropertyName { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public double Area { get; set; }

    public string PropertyType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public ICollection<Booking>? Bookings { get; set; }

    public ICollection<Favorite>? Favorites { get; set; }

    public ICollection<Review>? Reviews { get; set; }

    [Required]
    public string Status { get; set; } = "Available";

    public ICollection<PropertyImage>? PropertyImages { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
