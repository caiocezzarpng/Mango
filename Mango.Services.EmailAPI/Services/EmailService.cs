using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Here is your Shopping Cart");
            message.AppendLine($"<br/>Total: {cartDTO.CartHeader.CartTotal}$");
            message.AppendLine("<br/>Items in the cart are the following: ");
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDTO.CartDetails)
            {
                message.Append("<li>");
                message.Append($"Product: {item.Product?.Name} ");
                message.Append($"Price: {item.Product?.Price} ");
                message.Append($"Quantity: {item.Count} ");
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
        }

        public async Task EmailRegistrationAndLog(RegistrationRequestDTO requestDTO)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine($"<br/>Hi {requestDTO.Name}");
            message.AppendLine("<br/>Welcome to our App!");

            await LogAndEmail(message.ToString(), requestDTO.Email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();
                return true;

            } catch(Exception)
            {
                return false;
            }
        }
    }
}