using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTOs;

namespace Mango.Services.OrderAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDTO, CartHeaderDTO>().ForMember(
                    dest => dest.CartTotal,
                    u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

                config.CreateMap<CartDetailsDTO, OrderDetailsDTO > ().ForMember(
                        dest => dest.ProductName,
                        u => u.MapFrom(src => src.Product.Name))
                    .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

                config.CreateMap<OrderDetailsDTO, CartDetailsDTO>();

                config.CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDTO>().ReverseMap();
            });

            return MappingConfig;
        }
    }
}