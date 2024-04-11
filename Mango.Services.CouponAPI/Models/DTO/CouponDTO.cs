namespace Mango.Services.CouponAPI.Models.DTO
{
    public class CouponDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
