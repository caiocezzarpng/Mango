using Mango.Web.Models.DTOs;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProductsAsync();
        Task<ResponseDTO?> GetProductByIdAsync(long id);
        Task<ResponseDTO?> GetProductByNameAsync(string name);
        Task<ResponseDTO?> GetProductsByCategoryNameAsync(string categoryName);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductAsync(long id);
    }
}
