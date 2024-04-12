namespace Mango.Web.Service.IService
{
    public interface ItokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
