using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }
    }
}
