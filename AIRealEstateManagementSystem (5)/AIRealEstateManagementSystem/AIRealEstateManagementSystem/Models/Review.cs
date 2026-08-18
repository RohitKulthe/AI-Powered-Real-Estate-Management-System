using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int PropertyId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public Property? Property { get; set; }

        public Customer? Customer { get; set; }
    }
}