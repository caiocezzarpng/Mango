using Mango.Web.Models.DTOs;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetAllCouponsAsync();
        Task<ResponseDTO?> GetCouponByCodeAsync(string code);
        Task<ResponseDTO?> GetCouponByIdAsync(long id);
        Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDTO?> DeleteCouponAsync(long id);
    }
}
