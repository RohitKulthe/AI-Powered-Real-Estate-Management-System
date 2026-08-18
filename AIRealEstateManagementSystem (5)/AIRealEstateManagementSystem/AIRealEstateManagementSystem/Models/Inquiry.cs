using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models
{
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        public int PropertyId { get; set; }

        public Property? Property { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime InquiryDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";
    }
}