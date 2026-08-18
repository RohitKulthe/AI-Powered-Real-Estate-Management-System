using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    public int PropertyId { get; set; }

    public int CustomerId { get; set; }

    [Required]
    public DateTime VisitDate { get; set; }

    [Required]
    public TimeSpan VisitTime { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Property? Property { get; set; }

    public Customer? Customer { get; set; }


}
