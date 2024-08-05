namespace Mango.Services.OrderAPI.Models.DTOs
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = String.Empty;
    }
}