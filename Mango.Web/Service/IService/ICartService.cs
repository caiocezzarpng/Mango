﻿using Mango.Web.Models.DTOs;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIdAsync(string userId);
        Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO);
        Task<ResponseDTO?> RemoveFromCartAsync(long cartDetailsId);
        Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO);
        Task<ResponseDTO?> EmailCart(CartDTO cartDTO);
    }
}
