using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0.1, 1000)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
