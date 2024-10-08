﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.DTOs
{
    public class CartHeaderDTO
    {
        public long Id { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double CartTotal { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Telephone { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
