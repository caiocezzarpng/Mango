namespace Mango.Services.OrderAPI.Models.DTOs
{
    public class CartDetailsDTO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public long ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
    }
}