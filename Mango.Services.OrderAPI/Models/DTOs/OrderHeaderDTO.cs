namespace Mango.Services.OrderAPI.Models.DTOs
{
    public class OrderHeaderDTO
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }
        public IEnumerable<OrderDetailsDTO> OrderDetails { get; set; }
    }
}
