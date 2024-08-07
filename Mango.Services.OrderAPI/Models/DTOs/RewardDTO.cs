namespace Mango.Services.OrderAPI.Models.DTOs
{
    public class RewardDTO
    {
        public string UserId { get; set; }
        public int RewardActivity { get; set; }
        public long OrderId { get; set; }   
    }
}
