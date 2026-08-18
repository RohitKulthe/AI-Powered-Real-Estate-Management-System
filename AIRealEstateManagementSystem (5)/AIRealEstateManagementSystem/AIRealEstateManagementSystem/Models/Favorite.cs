using System.ComponentModel.DataAnnotations;

namespace AIRealEstateManagementSystem.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        public int PropertyId { get; set; }

        public int CustomerId { get; set; }

        public Property? Property { get; set; }

        public Customer? Customer { get; set; }
    }
}