using Mango.Services.ShoppingCartAPI.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public long Id { get; set; }
        public long CartHeaderId { get; set; }

        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public long ProductId { get; set; }

        [NotMapped]
        public ProductDTO Product { get; set; }
        public int Count { get; set; }
    }
}
