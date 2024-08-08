namespace Mango.Services.RewardAPI.Message
{
    public class RewardsMessage
    {
        public long Id { get; set; }
        public int RewardActivity { get; set; }
        public string UserId { get; set; }
        public long OrderId { get; set; }
    }
}