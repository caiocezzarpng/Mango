﻿using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Coupon = Mango.Services.CouponAPI.Models.Coupon;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:long}")]
        public ResponseDTO Get(long id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.Id == id);
                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex) 
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO Get(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.Code.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Post([FromBody] CouponDTO couponDTO) 
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _db.Coupons.Add(obj);
                _db.SaveChanges();

                var options = new CouponCreateOptions
                {
                    AmountOff = (long)(couponDTO.DiscountAmount*100),
                    Name = couponDTO.Code,
                    Currency = "usd",
                    Id = couponDTO.Code,
                };

                var service = new CouponService();
                service.Create(options);

                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _db.Coupons.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Delete(long id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.Id == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();

                var service = new CouponService();
                service.Delete(obj.Code);

                _response.Message = "Coupon has been deleted!";
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
