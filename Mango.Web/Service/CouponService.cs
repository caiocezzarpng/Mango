using Mango.Services.CouponAPI.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utils;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = couponDTO,
                Url = StaticDetails.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(long id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase+"/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetCouponByCodeAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/GetByCode/" + code
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(long id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = couponDTO,
                Url = StaticDetails.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
