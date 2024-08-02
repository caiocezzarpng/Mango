using Mango.Web.Models.DTOs;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(long cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTO? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            
            if (response != null && response.Success)
            {
                TempData["success"] = "Item has been removed from cart successfully.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            ResponseDTO? response = await _cartService.ApplyCouponAsync(cartDTO);
            if (response != null && response.Success)
            {
                TempData["success"] = "Coupon applied successfully.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();   
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDTO cartDTO)
        {
            CartDTO cart = await LoadCartDTOBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDTO? response = await _cartService.EmailCart(cart);
            if (response != null && response.Success)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            cartDTO.CartHeader.CouponCode = string.Empty;
            ResponseDTO? response = await _cartService.ApplyCouponAsync(cartDTO);
            if (response != null && response.Success)
            {
                TempData["success"] = "Coupon removed successfully.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [Authorize]
        private async Task<CartDTO> LoadCartDTOBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTO? response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.Success)
            {
                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
                return cartDTO;
            }
            return new CartDTO();
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }
    }
}