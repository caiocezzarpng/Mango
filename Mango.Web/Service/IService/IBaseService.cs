using Mango.Web.Models.DTOs;

namespace Mango.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBearer = true);
    }
}
