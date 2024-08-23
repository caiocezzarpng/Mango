using Mango.Web.Models.DTOs;
using Mango.Web.Service.IService;
using Mango.Web.Utils;
using System.Xml.Linq;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = productDTO,
                Url = StaticDetails.ProductAPIBase + "/api/product",
                ContentType = StaticDetails.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(long id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(long id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDTO?> GetProductByNameAsync(string name)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product/GetByName/" + name
            });
        }

        public async Task<ResponseDTO?> GetProductsByCategoryNameAsync(string categoryName)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "/api/product/GetByCategory/" + categoryName
            });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = productDTO,
                Url = StaticDetails.ProductAPIBase + "/api/product",
                ContentType = StaticDetails.ContentType.MultipartFormData
            });
        }
    }
}
