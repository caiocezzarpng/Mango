namespace Mango.Web.Models.DTOs
{
    public class OrderDetailsDTO
    {
        public long OrderDetailsId { get; set; }
        public long OrderHeaderId { get; set; }
        public long ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}