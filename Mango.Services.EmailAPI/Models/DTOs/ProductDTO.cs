﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.EmailAPI.Models.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        [Range(1, 100)]
        public int count { get; set; } = 1;
        public string? ImageLocalPath { get; set; }
    }
}