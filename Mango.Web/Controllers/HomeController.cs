using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.DTOs;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? list = new();

            ResponseDTO? response = await _productService.GetAllProductsAsync();

            if (response != null && response.Success)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }   

            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(long productId)
        {
            ProductDTO? model = new();

            ResponseDTO? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.Success)
            {
                model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
        {
            CartDTO cartDTO = new()
            {
                CartHeader = new()
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDTO cartDetailsDTO = new()
            {
                Count = productDTO.count,
                ProductId = productDTO.Id
            };

            List<CartDetailsDTO> cartDetailsDTOs = new() { cartDetailsDTO };
            cartDTO.CartDetails = cartDetailsDTOs;



            ResponseDTO? response = await _cartService.UpsertCartAsync(cartDTO);

            if (response != null && response.Success)
            {
                TempData["success"] = "Item has been added to the cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}