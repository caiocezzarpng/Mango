using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.DTOs;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);

        Task EmailRegistrationAndLog(RegistrationRequestDTO requestDTO);

        Task LogOrderPlaced(RewardsMessage rewardsMessage);
    }
}
