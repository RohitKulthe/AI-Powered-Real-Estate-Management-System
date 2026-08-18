using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIRealEstateManagementSystem.Models
{
    public class PropertyImage
    {
        [Key]
        public int ImageId { get; set; }

        public int PropertyId { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public Property? Property { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}