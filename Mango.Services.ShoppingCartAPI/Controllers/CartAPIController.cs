using AutoMapper;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private AppDbContext _db;
        private IProductService _productService;
        private ICouponService _couponService;
        private IConfiguration _configuration;
        private IMessageBus _messageBus;

        public CartAPIController(IMapper mapper, AppDbContext dbContext, IProductService productService, ICouponService couponService, IConfiguration configuration, IMessageBus messageBus)
        {
            _mapper = mapper;
            _db = dbContext;
            _productService = productService;
            _couponService = couponService;
            _configuration = configuration;
            _messageBus = messageBus;
            _response = new ResponseDTO();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartHeader cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
                List<CartDetails> cartDetails = _db.CartDetails.Where(u => u.CartHeaderId == cartHeader.Id).ToList();
                CartDTO cartDTO = new CartDTO
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(cartHeader),
                    CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(cartDetails)
                };

                IEnumerable<ProductDTO> productDTOs = await _productService.GetProducts();

                foreach (var item in cartDTO.CartDetails)
                {
                    item.Product = productDTOs.FirstOrDefault(u => u.Id == item.ProductId);
                    cartDTO.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                if (!string.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
                {
                    CouponDTO couponDTO = await _couponService.GetCoupon(cartDTO.CartHeader.CouponCode);
                    if (couponDTO != null && cartDTO.CartHeader.CartTotal >= couponDTO.MinAmount)
                    {
                        cartDTO.CartHeader.CartTotal -= couponDTO.DiscountAmount;
                        cartDTO.CartHeader.Discount = couponDTO.DiscountAmount;
                    }
                }

                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.Success = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                cartFromDb.CouponCode = cartDTO.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.Success = false;
            }
            return _response;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDTO cartDTO)
        {
            try
            {
                await _messageBus.PublishMessage(cartDTO, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.Success = false;
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.Id;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDTO.CartDetails.First().ProductId && u.CartHeaderId == cartHeaderFromDb.Id);

                    if (cartDetailsFromDb == null)
                    {
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cartDTO.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetails.First().Id = cartDetailsFromDb.Id;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }

                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.Success = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] long cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(u => u.Id == cartDetailsId);

                int totalCountOfCartItems = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).ToList().Count;
                _db.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.Id == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.Success = false;
            }
            return _response;
        }
    }
}
