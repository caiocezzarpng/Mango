namespace Mango.Services.ProductAPI.Models.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
