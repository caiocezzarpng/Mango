namespace Mango.Services.OrderAPI.Models.DTOs
{
    public class OrderDetailsDTO
    {
        public long Id { get; set; }
        public long OrderHeaderId { get; set; }
        public long ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}