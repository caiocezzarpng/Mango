using Mango.Services.OrderAPI.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public long Id { get; set; }

        public long OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }

        public long ProductId { get; set; }

        [NotMapped]
        public ProductDTO? Product { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}