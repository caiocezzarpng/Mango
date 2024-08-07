namespace Mango.Services.RewardAPI.Models.DTOs
{
    public class RewardDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public DateTime RewardDate { get; set; }
        public int RewardActivity { get; set; }
        public long OrderId { get; set; }
    }
}
