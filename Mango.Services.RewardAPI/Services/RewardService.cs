using Mango.Services.RewardAPI.Data;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.RewardAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<bool> UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Reward reward = new()
                {
                    UserId = rewardsMessage.UserId,
                    OrderId = rewardsMessage.OrderId,
                    RewardActivity = rewardsMessage.RewardActivity,
                    RewardDate = DateTime.Now
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(reward);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}