using Mango.Web.Utils;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        [Range(1, 100)]
        public int count { get; set; } = 1;

        [MaxFileSize(4)]
        [AllowedExtensions([".jpg", ".png"])]
        public IFormFile? Image { get; set; }
        public string? ImageLocalPath { get; set; }
    }
}