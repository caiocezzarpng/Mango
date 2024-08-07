using Mango.Web.Models.DTOs;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO?> CreateOrderAsync(CartDTO cartDto);
        Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDto);
        Task<ResponseDTO?> ValidateStripeSession(long orderHeaderId);
    }
}
